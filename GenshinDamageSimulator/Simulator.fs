namespace GenshinDamageSimulator

open EventHandling

exception NoHistoryException of string

[<Struct>]
type SimulationState =
    { Combatants: Map<uint32, (BattleNpc * BattleNpcState)>
      Party: Party
      History: SimulationState list }

module Simulator =
    let AddCombatant state combatantId bNpc =
        { state with Combatants = state.Combatants.Add (combatantId, bNpc); History = state :: state.History }

    let AddPartyMember state bNpc =
        { state with Party = bNpc :: state.Party; History = state :: state.History }

    let DoEvent state event attackerId defenderId =
        let defenderDamage = match state.Combatants.TryFind attackerId with
                                | Some (attacker, _) -> match state.Combatants.TryFind defenderId with
                                                        | Some (defender, _) -> handleEvent event attacker defender
                                                        | None -> 0u
                                | None -> 0u
        let updateFn v =
            match v with
            | Some (defender, defenderState) -> Some(defender, { defenderState with Hp = defenderState.Hp - defenderDamage })
            | None -> None
        { state with Combatants = state.Combatants.Change (defenderId, updateFn); History = state :: state.History }

    let StepBack state =
        match state.History with
        | head :: tail -> { Combatants = head.Combatants; Party = head.Party; History = tail }
        | _ -> state

    let Create =
        { Combatants = Map.empty
          Party = []
          History = [] }

// This is the C# interface for the simulator.
type SimulationState with
    static member Create() = Simulator.Create

    member this.AddCombatant combatantId bNpc bNpcState = Simulator.AddCombatant this combatantId (bNpc, bNpcState)

    member this.AddPartyMember bNpc = Simulator.AddPartyMember this bNpc

    member this.DoEvent event attackerId defenderId = Simulator.DoEvent this event attackerId defenderId

    member this.StepBack() = Simulator.StepBack this