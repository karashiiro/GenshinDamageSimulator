namespace GenshinDamageSimulator

open EventHandling

exception InvalidEventResultException of string * GameEventResult
exception CombatantNotFoundException of string * CombatantId

[<Struct>]
type SimulationState =
    { Combatants: Map<uint32, (BattleNpc * BattleNpcState)>
      Party: Party
      LastEventResult: GameEventResult
      Timestamp: int64<ms>
      History: SimulationState list }

module Simulator =
    let elapse state t =
        { state with Timestamp = state.Timestamp + t }

    let addCombatant state (bNpc, bNpcState) =
        { state with
            Combatants = state.Combatants.Add (bNpcState.Id, (bNpc, bNpcState))
            LastEventResult = CombatantAddResult ({ BNpc = (bNpc, bNpcState) })
            History = state :: state.History }

    let removeCombatant state combatantId =
        let cOpt = state.Combatants.TryFind combatantId
        match cOpt with
        | Some _
            -> { state with
                    Combatants = state.Combatants.Remove combatantId
                    LastEventResult = CombatantRemoveResult ({ TargetId = combatantId })
                    History = state :: state.History }
        | None -> raise (CombatantNotFoundException("Combatant not found", combatantId))

    let addPartyMember state combatantId =
        let cOpt = state.Combatants.TryFind combatantId
        match cOpt with
        | Some (combatant, combatantState)
            -> { state with
                    Party = state.Party.Add (combatantState.Id, combatant)
                    LastEventResult = PartyAddResult ({ TargetId = combatantState.Id })
                    History = state :: state.History }
        | None -> raise (CombatantNotFoundException("Combatant not found", combatantId))

    let removePartyMember state combatantId =
        let cOpt = state.Combatants.TryFind combatantId
        match cOpt with
        | Some _
            -> { state with
                    Party = state.Party.Remove combatantId
                    LastEventResult = PartyRemoveResult ({ TargetId = combatantId })
                    History = state :: state.History }
        | None -> raise (CombatantNotFoundException("Combatant not found", combatantId))

    let combatantantUpdateFn f value =
        match value with
        | Some matchedValue
            -> Some(f matchedValue)
        | _ -> None

    let applyDamageResult state damageResult targetId =
        let updateFnTarget =
            combatantantUpdateFn (fun (target, targetState) -> (target, { targetState with Hp = targetState.Hp - damageResult.DamageAmount }))
        { state with
            Combatants = state.Combatants.Change (targetId, updateFnTarget)
            LastEventResult = DamageResult (damageResult)
            History = state :: state.History }

    let doEvent state event sourceId targetId =
        let sourceOption = state.Combatants.TryFind sourceId
        let targetOption = state.Combatants.TryFind targetId
        let eventResult = handleEvent event sourceOption targetOption
        match eventResult with
        | ElapseResult r -> elapse state r.TimeElapsed
        | CombatantAddResult r -> addCombatant state r.BNpc
        | CombatantRemoveResult r -> removeCombatant state r.TargetId
        | PartyAddResult r -> addPartyMember state r.TargetId
        | PartyRemoveResult r -> removePartyMember state r.TargetId
        | DamageResult r -> applyDamageResult state r targetId
        | _ -> raise (InvalidEventResultException("No handler present for event result", eventResult))

    let stepBack state =
        match state.History with
        | _ :: nextHead :: _ -> nextHead
        | _ -> state

    let genesis =
        { Combatants = Map.empty
          Party = Map.empty
          LastEventResult = GenesisResult (GenesisEventResult ())
          Timestamp = 0
          History = [] }

// This is the C# interface for the simulator.
type SimulationState with
    static member Create() = Simulator.genesis

    member this.DoEvent event sourceId targetId = Simulator.doEvent this event sourceId targetId

    member this.StepBack() = Simulator.stepBack this