namespace GenshinDamageSimulator

open EventHandling

[<Struct>]
type SimulationState =
    { Combatants: Map<uint32, (BattleNpc * BattleNpcState)>
      Party: Party
      LastEventResult: GameEventResult option
      History: SimulationState list }

module Simulator =
    let AddCombatant state (bNpc, bNpcState) =
        { state with Combatants = state.Combatants.Add (bNpcState.Id, (bNpc, bNpcState)); History = state :: state.History }

    let AddPartyMember state bNpc =
        { state with Party = bNpc :: state.Party; History = state :: state.History }

    let DoEvent state event attackerId defenderId =
        let attackerOption = state.Combatants.TryFind attackerId
        let defenderOption = state.Combatants.TryFind defenderId
        let eventResult = match (attackerOption, defenderOption) with
                          | (Some attacker, Some defender) -> Some(handleEvent event attacker defender)
                          | _ -> None
        let updateFn v =
            match (v, eventResult) with
            | (Some (defender, defenderState), Some (GameEventResult.DamageResult r))
                -> Some(defender, { defenderState with Hp = defenderState.Hp - r.DamageAmount })
            | _ -> None
        { state with Combatants = state.Combatants.Change (defenderId, updateFn); LastEventResult = eventResult; History = state :: state.History }

    let StepBack state =
        match state.History with
        | _ :: nextHead :: _ -> nextHead
        | _ -> state

    let Create =
        { Combatants = Map.empty
          Party = []
          LastEventResult = None
          History = [] }

// This is the C# interface for the simulator.
type SimulationState with
    static member Create() = Simulator.Create

    member this.AddCombatant bNpc bNpcState = Simulator.AddCombatant this (bNpc, bNpcState)

    member this.AddPartyMember bNpc = Simulator.AddPartyMember this bNpc

    member this.DoEvent event attackerId defenderId = Simulator.DoEvent this event attackerId defenderId

    member this.StepBack() = Simulator.StepBack this