namespace GenshinDamageSimulator

open EntityTypes
open PartyTypes

module Simulator =
    [<Struct>]
    type SimulationState =
        { Combatants: (BattleNpc * BattleNpcState) list
          Party: Party }

    let AddCombatant state bNpc =
        { state with Combatants = bNpc :: state.Combatants }

    let CreateSimulation () =
        { Combatants = []
          Party = { Members = [] } }