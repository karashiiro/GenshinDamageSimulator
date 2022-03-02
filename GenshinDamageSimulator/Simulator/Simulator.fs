namespace GenshinDamageSimulator

exception InvalidEventResultException of string * GameEventResult
exception EntityNotFoundException of string * EntityId

type SimulationState =
    { Ids: Map<EntityId, bool>
      Combatants: Map<EntityId, (Entity * EntityState)>
      Party: Party
      LastEventResult: GameEventResult
      TimestampMs: int64
      History: SimulationState list }

module Simulator =
    let rec freeId state id =
        if state.Ids |> Map.containsKey id then
            freeId state (id + 1)
        else
            id

    let elapse (result: ElapseEventResult) state =
        { state with 
            TimestampMs = state.TimestampMs + result.TimeElapsedMs
            LastEventResult = result |> ElapseResult }

    let addCombatant (result: CombatantAddEventResult) state =
        { state with
            Combatants = state.Combatants.Add (result.EntityState.Id, (result.Entity, result.EntityState))
            LastEventResult = result |> CombatantAddResult
            History = state :: state.History }

    let removeCombatant (result: CombatantRemoveEventResult) state =
        let cOpt = state.Combatants.TryFind result.TargetId
        match cOpt with
        | Some _
            -> { state with
                    Combatants = state.Combatants.Remove result.TargetId
                    LastEventResult = result |> CombatantRemoveResult
                    History = state :: state.History }
        | None -> raise (EntityNotFoundException("Combatant not found.", result.TargetId))

    let addPartyMember (result: PartyAddEventResult) state =
        let cOpt = state.Combatants.TryFind result.TargetId
        match cOpt with
        | Some (combatant, _)
            -> match combatant with
               | CharacterEntity c ->
                    { state with
                        Party = state.Party.Add (result.TargetId, c)
                        LastEventResult = result |> PartyAddResult
                        History = state :: state.History }
               | _ -> raise (EntityNotFoundException("Character not found.", result.TargetId))
        | None -> raise (EntityNotFoundException("Character not found.", result.TargetId))

    let removePartyMember (result: PartyRemoveEventResult) state =
        let cOpt = state.Combatants.TryFind result.TargetId
        match cOpt with
        | Some _
            -> { state with
                    Party = state.Party.Remove result.TargetId
                    LastEventResult = result |> PartyRemoveResult
                    History = state :: state.History }
        | None -> raise (EntityNotFoundException("Character not found.", result.TargetId))

    let combatantantUpdateFn f value =
        match value with
        | Some matchedValue
            -> Some(f matchedValue)
        | _ -> None

    let damageResultFn (result: DamageEventResult) (target, targetState) =
        match result.DamageAura with
        | Some aura
            -> (target, { targetState with
                            Hp = targetState.Hp - result.DamageAmount
                            ElementalAuras = (ElementalAuraState.unwrap targetState.ElementalAuras).Change((ElementalAura.unwrap aura).Element, fun _ -> Some(aura)) |> ElementalAuraState })
        | None -> (target, { targetState with Hp = targetState.Hp - result.DamageAmount })

    let applyDamageResult (result: DamageEventResult) state =
        let updateFnTarget =
            combatantantUpdateFn (damageResultFn result)
        { state with
            Combatants = state.Combatants.Change (result.TargetId, updateFnTarget)
            LastEventResult = result |> DamageResult
            History = state :: state.History }

    let doEventOpt sourceOption targetOption event =
        let eventResult = EventHandling.handleEvent event sourceOption targetOption
        match eventResult with
        | ElapseResult r -> elapse r
        | CombatantAddResult r -> addCombatant r
        | CombatantRemoveResult r -> removeCombatant r
        | PartyAddResult r -> addPartyMember r
        | PartyRemoveResult r -> removePartyMember r
        | DamageResult r -> applyDamageResult r
        | _ -> raise (InvalidEventResultException("No handler present for the produced event result.", eventResult))

    let doEvent state sourceId targetId =
        let sourceOption = state.Combatants.TryFind sourceId
        let targetOption = state.Combatants.TryFind targetId
        let eventOptFn e = doEventOpt sourceOption targetOption e state
        eventOptFn

    let stepBack state =
        match state.History with
        | head :: _ -> head
        | _ -> state

    let origin =
        { Ids = Map.empty
          Combatants = Map.empty
          Party = Map.empty
          LastEventResult = OriginResult (OriginEventResult ())
          TimestampMs = 0
          History = [] }

// This is the C# interface for the simulator.
type SimulationState with
    /// Creates a new simulator with the origin state.
    static member Create () = Simulator.origin

    /// Gets an unused entity ID from the simulation state.
    member this.FreeId () =
        Simulator.freeId this (EntityId.create 0)

    /// Executes a new elapse event on the current simulation state, returning a new simulation state
    /// with the current state in the history stack.
    member this.Elapse (timeElapsedMs: int64) =
        Simulator.doEvent this EntityId.none EntityId.none (Elapse ({ TimeElapsedMs = timeElapsedMs }))

    /// Executes a new combatant add event on the current simulation state, returning a new simulation state
    /// with the current state in the history stack.
    member this.CombatantAdd (entity: Entity) (entityState: EntityState) =
        if isNull (box entity) then nullArg "entity"
        if isNull (box entityState) then nullArg "entityState"
        Simulator.doEvent this EntityId.none EntityId.none (CombatantAdd ({ Entity = entity; EntityState = entityState }))

    /// Executes a new combatant remove event on the current simulation state, returning a new simulation state
    /// with the current state in the history stack.
    member this.CombatantRemove (targetId: EntityId) =
        Simulator.doEvent this EntityId.none targetId (CombatantRemove (CombatantRemoveEvent ()))

    /// Executes a new party add event on the current simulation state, returning a new simulation state
    /// with the current state in the history stack.
    member this.PartyAdd (targetId: EntityId) =
        if isNull (box targetId) then nullArg "targetId"
        Simulator.doEvent this EntityId.none targetId (PartyAdd (PartyAddEvent ()))

    /// Executes a new party remove event on the current simulation state, returning a new simulation state
    /// with the current state in the history stack.
    member this.PartyRemove (targetId: EntityId) =
        if isNull (box targetId) then nullArg "targetId"
        Simulator.doEvent this EntityId.none targetId (PartyRemove (PartyRemoveEvent ()))

    /// Executes a new talent damage event on the current simulation state, returning a new simulation state
    /// with the current state in the history stack.
    member this.TalentDamage (damageType: DamageType) (damageStat: BaseStat) (damageStatMultiplier: float32) (critical: Critical) (sourceId: EntityId) (targetId: EntityId) =
        if isNull (box damageType) then nullArg "damageType"
        if isNull (box damageStat) then nullArg "damageStat"
        if isNull (box damageStatMultiplier) then nullArg "damageStatMultiplier"
        if isNull (box critical) then nullArg "critical"
        if isNull (box sourceId) then nullArg "sourceId"
        if isNull (box targetId) then nullArg "targetId"
        let event = { DamageType = damageType; DamageStat = damageStat; DamageStatMultiplier = damageStatMultiplier; Critical = critical }
        Simulator.doEvent this sourceId targetId (TalentDamage event)

    /// Executes a new talent heal event on the current simulation state, returning a new simulation state
    /// with the current state in the history stack.
    member this.TalentHeal (healStat: BaseStat) (healStatMultiplier: float32) (sourceId: EntityId) (targetId: EntityId) =
        if isNull (box healStat) then nullArg "healStat"
        if isNull (box healStatMultiplier) then nullArg "healStatMultiplier"
        if isNull (box sourceId) then nullArg "sourceId"
        if isNull (box targetId) then nullArg "targetId"
        let event = { HealStat = healStat; HealStatMultiplier = healStatMultiplier }
        Simulator.doEvent this sourceId targetId (TalentHeal event)

    /// Returns the top state from the history stack, or the origin state if the history stack is empty.
    member this.StepBack() = Simulator.stepBack this