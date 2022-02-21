namespace GenshinDamageSimulator

exception InvalidEventResultException of string * GameEventResult
exception EntityNotFoundException of string * EntityId

type SimulationState =
    { Combatants: Map<EntityId, (Entity * EntityState)>
      Party: Party
      LastEventResult: GameEventResult
      TimestampMs: int64
      History: SimulationState list }

module Simulator =
    let elapse state t =
        { state with TimestampMs = state.TimestampMs + t }

    let addCombatant state bNpc bNpcState =
        { state with
            Combatants = state.Combatants.Add (bNpcState.Id, (bNpc, bNpcState))
            LastEventResult = CombatantAddResult ({ BNpc = bNpc; BNpcState = bNpcState })
            History = state :: state.History }

    let removeCombatant state combatantId =
        let cOpt = state.Combatants.TryFind combatantId
        match cOpt with
        | Some _
            -> { state with
                    Combatants = state.Combatants.Remove combatantId
                    LastEventResult = CombatantRemoveResult ({ TargetId = combatantId })
                    History = state :: state.History }
        | None -> raise (EntityNotFoundException("Combatant not found", combatantId))

    let addPartyMember state combatantId =
        let cOpt = state.Combatants.TryFind combatantId
        match cOpt with
        | Some (combatant, combatantState)
            -> match combatant with
               | CharacterEntity c ->
                    { state with
                        Party = state.Party.Add (combatantState.Id, c)
                        LastEventResult = PartyAddResult ({ TargetId = combatantState.Id })
                        History = state :: state.History }
               | _ -> raise (EntityNotFoundException("Character not found", combatantId))
        | None -> raise (EntityNotFoundException("Character not found", combatantId))

    let removePartyMember state combatantId =
        let cOpt = state.Combatants.TryFind combatantId
        match cOpt with
        | Some _
            -> { state with
                    Party = state.Party.Remove combatantId
                    LastEventResult = PartyRemoveResult ({ TargetId = combatantId })
                    History = state :: state.History }
        | None -> raise (EntityNotFoundException("Character not found", combatantId))

    let combatantantUpdateFn f value =
        match value with
        | Some matchedValue
            -> Some(f matchedValue)
        | _ -> None

    let damageResultFn damageResult (target, targetState) =
        match damageResult.DamageAura with
        | Some aura
            -> (target, { targetState with
                            Hp = targetState.Hp - damageResult.DamageAmount
                            ElementalAuras = targetState.ElementalAuras.Change((ElementalAura.unwrapAuraData aura).Element, fun _ -> Some(aura)) })
        | None -> (target, { targetState with Hp = targetState.Hp - damageResult.DamageAmount })

    let applyDamageResult state damageResult targetId =
        let updateFnTarget =
            combatantantUpdateFn (damageResultFn damageResult)
        { state with
            Combatants = state.Combatants.Change (targetId, updateFnTarget)
            LastEventResult = DamageResult (damageResult)
            History = state :: state.History }

    let doEvent state event sourceId targetId =
        let sourceOption = state.Combatants.TryFind sourceId
        let targetOption = state.Combatants.TryFind targetId
        let eventResult = EventHandling.handleEvent event sourceOption targetOption
        match eventResult with
        | ElapseResult r -> elapse state r.TimeElapsedMs
        | CombatantAddResult r -> addCombatant state r.BNpc r.BNpcState
        | CombatantRemoveResult r -> removeCombatant state r.TargetId
        | PartyAddResult r -> addPartyMember state r.TargetId
        | PartyRemoveResult r -> removePartyMember state r.TargetId
        | DamageResult r -> applyDamageResult state r targetId
        | _ -> raise (InvalidEventResultException("No handler present for event result", eventResult))

    let stepBack state =
        match state.History with
        | head :: _ -> head
        | _ -> state

    let origin =
        { Combatants = Map.empty
          Party = Map.empty
          LastEventResult = OriginResult (OriginEventResult ())
          TimestampMs = 0
          History = [] }

// This is the C# interface for the simulator.
type SimulationState with
    /// Creates a new simulator with the origin state.
    static member Create() = Simulator.origin

    /// Executes an event on the current simulation state, returning a new simulation state
    /// with the current state in the history stack.
    member this.DoEvent(event: GameEvent, sourceId: EntityId, targetId: EntityId) =
        if isNull (box event) then nullArg "event"
        Simulator.doEvent this event sourceId targetId

    /// Returns the top state from the history stack, or the origin state if the history stack is empty.
    member this.StepBack() = Simulator.stepBack this