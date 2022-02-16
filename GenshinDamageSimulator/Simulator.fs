namespace GenshinDamageSimulator

open EventHandling

[<Struct>]
type SimulationState =
    { Combatants: Map<uint32, (BattleNpc * BattleNpcState)>
      Party: Party
      LastEventResult: GameEventResult
      Timestamp: int64<ms>
      History: SimulationState list }

module Simulator =
    let AddCombatant state (bNpc, bNpcState) =
        { state with Combatants = state.Combatants.Add (bNpcState.Id, (bNpc, bNpcState)); History = state :: state.History }

    let AddPartyMember state bNpc =
        { state with Party = bNpc :: state.Party; History = state :: state.History }

    let DoEvent state event sourceId targetId =
        let sourceOption = state.Combatants.TryFind sourceId
        let targetOption = state.Combatants.TryFind targetId
        let eventResult = handleEvent event sourceOption targetOption
        let updateFn v =
            match (v, eventResult) with
            | (Some (defender, defenderState), DamageResult r)
                -> Some(defender, { defenderState with Hp = defenderState.Hp - r.DamageAmount })
            | _ -> None
        { state with Combatants = state.Combatants.Change (targetId, updateFn); LastEventResult = eventResult; History = state :: state.History }

    let StepBack state =
        match state.History with
        | _ :: nextHead :: _ -> nextHead
        | _ -> state

    let Create =
        { Combatants = Map.empty
          Party = []
          LastEventResult = GenesisResult (GenesisEventResult ())
          Timestamp = 0
          History = [] }

// This is the C# interface for the simulator.
type SimulationState with
    static member Create() = Simulator.Create

    member this.AddCombatant bNpc bNpcState = Simulator.AddCombatant this (bNpc, bNpcState)

    member this.AddPartyMember bNpc = Simulator.AddPartyMember this bNpc

    member this.DoEvent event attackerId defenderId = Simulator.DoEvent this event attackerId defenderId

    member this.StepBack() = Simulator.StepBack this