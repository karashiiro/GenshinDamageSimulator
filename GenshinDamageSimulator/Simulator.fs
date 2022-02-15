﻿namespace GenshinDamageSimulator

[<Struct>]
type SimulationState =
    { Combatants: (BattleNpc * BattleNpcState) list
      Party: Party }

module Simulator =
    let AddCombatant state bNpc =
        { state with Combatants = bNpc :: state.Combatants }

    let AddPartyMember state bNpc =
        { state with Party = bNpc :: state.Party }

    let Create =
        { Combatants = []
          Party = [] }

// This is the C# interface for the simulator.
type SimulationState with
    static member Create() = Simulator.Create

    member this.AddCombatant bNpc bNpcState = Simulator.AddCombatant this (bNpc, bNpcState)

    member this.AddPartyMember bNpc = Simulator.AddPartyMember this bNpc