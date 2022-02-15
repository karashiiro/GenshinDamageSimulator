namespace GenshinDamageSimulator

open StatTypes

module Stat =
    // Flat modifiers
    let calcModifierHpFlat modifier =
        match modifier with
        | FlatStatModifier sm -> match sm.Type with
                                    | FlatStatType.Hp -> sm.Value
                                    | _ -> 0u
        | _ -> 0u

    let calcModifierAttackFlat modifier =
        match modifier with
        | FlatStatModifier sm -> match sm.Type with
                                    | FlatStatType.Attack -> sm.Value
                                    | _ -> 0u
        | _ -> 0u

    let calcModifierDefenseFlat modifier =
        match modifier with
        | FlatStatModifier sm -> match sm.Type with
                                    | FlatStatType.Defense -> sm.Value
                                    | _ -> 0u
        | _ -> 0u

    let calcModifierElementalMasteryFlat modifier =
        match modifier with
        | FlatStatModifier sm -> match sm.Type with
                                    | FlatStatType.ElementalMastery -> sm.Value
                                    | _ -> 0u
        | _ -> 0u

    // Percent modifiers
    let calcModifierHpPercent modifier =
        match modifier with
        | PercStatModifier sm -> match sm.Type with
                                    | PercStatType.Hp -> sm.Value
                                    | _ -> 0.0f
        | _ -> 0.0f

    let calcModifierAttackPercent modifier =
        match modifier with
        | PercStatModifier sm -> match sm.Type with
                                    | PercStatType.Attack -> sm.Value
                                    | _ -> 0.0f
        | _ -> 0.0f

    let calcModifierDefensePercent modifier =
        match modifier with
        | PercStatModifier sm -> match sm.Type with
                                    | PercStatType.Defense -> sm.Value
                                    | _ -> 0.0f
        | _ -> 0.0f

    let calcModifierEnergyRechargePercent modifier =
        match modifier with
        | PercStatModifier sm -> match sm.Type with
                                    | PercStatType.EnergyRecharge -> sm.Value
                                    | _ -> 0.0f
        | _ -> 0.0f

    let calcModifierCriticalHitPercent modifier =
        match modifier with
        | PercStatModifier sm -> match sm.Type with
                                    | PercStatType.CriticalHit -> sm.Value
                                    | _ -> 0.0f
        | _ -> 0.0f

    let calcModifierCriticalDamagePercent modifier =
        match modifier with
        | PercStatModifier sm -> match sm.Type with
                                    | PercStatType.CriticalDamage -> sm.Value
                                    | _ -> 0.0f
        | _ -> 0.0f

    let calcModifierPhysicalPercent modifier =
        match modifier with
        | PercStatModifier sm -> match sm.Type with
                                    | PercStatType.Physical -> sm.Value
                                    | _ -> 0.0f
        | _ -> 0.0f

    let calcModifierPyroPercent modifier =
        match modifier with
        | PercStatModifier sm -> match sm.Type with
                                    | PercStatType.Pyro -> sm.Value
                                    | _ -> 0.0f
        | _ -> 0.0f

    let calcModifierHydroPercent modifier =
        match modifier with
        | PercStatModifier sm -> match sm.Type with
                                    | PercStatType.Hydro -> sm.Value
                                    | _ -> 0.0f
        | _ -> 0.0f

    let calcModifierElectroPercent modifier =
        match modifier with
        | PercStatModifier sm -> match sm.Type with
                                    | PercStatType.Electro -> sm.Value
                                    | _ -> 0.0f
        | _ -> 0.0f

    let calcModifierCryoPercent modifier =
        match modifier with
        | PercStatModifier sm -> match sm.Type with
                                    | PercStatType.Cryo -> sm.Value
                                    | _ -> 0.0f
        | _ -> 0.0f

    let calcModifierAnemoPercent modifier =
        match modifier with
        | PercStatModifier sm -> match sm.Type with
                                    | PercStatType.Anemo -> sm.Value
                                    | _ -> 0.0f
        | _ -> 0.0f

    let calcModifierGeoPercent modifier =
        match modifier with
        | PercStatModifier sm -> match sm.Type with
                                    | PercStatType.Geo -> sm.Value
                                    | _ -> 0.0f
        | _ -> 0.0f

    let calcModifierDendroPercent modifier =
        match modifier with
        | PercStatModifier sm -> match sm.Type with
                                    | PercStatType.Dendro -> sm.Value
                                    | _ -> 0.0f
        | _ -> 0.0f

    let calcModifierPhysicalResPercent modifier =
        match modifier with
        | PercStatModifier sm -> match sm.Type with
                                    | PercStatType.PhysicalRes -> sm.Value
                                    | _ -> 0.0f
        | _ -> 0.0f

    let calcModifierPyroResPercent modifier =
        match modifier with
        | PercStatModifier sm -> match sm.Type with
                                    | PercStatType.PyroRes -> sm.Value
                                    | _ -> 0.0f
        | _ -> 0.0f

    let calcModifierHydroResPercent modifier =
        match modifier with
        | PercStatModifier sm -> match sm.Type with
                                    | PercStatType.HydroRes -> sm.Value
                                    | _ -> 0.0f
        | _ -> 0.0f

    let calcModifierElectroResPercent modifier =
        match modifier with
        | PercStatModifier sm -> match sm.Type with
                                    | PercStatType.ElectroRes -> sm.Value
                                    | _ -> 0.0f
        | _ -> 0.0f

    let calcModifierCryoResPercent modifier =
        match modifier with
        | PercStatModifier sm -> match sm.Type with
                                    | PercStatType.CryoRes -> sm.Value
                                    | _ -> 0.0f
        | _ -> 0.0f

    let calcModifierAnemoResPercent modifier =
        match modifier with
        | PercStatModifier sm -> match sm.Type with
                                    | PercStatType.AnemoRes -> sm.Value
                                    | _ -> 0.0f
        | _ -> 0.0f

    let calcModifierGeoResPercent modifier =
        match modifier with
        | PercStatModifier sm -> match sm.Type with
                                    | PercStatType.GeoRes -> sm.Value
                                    | _ -> 0.0f
        | _ -> 0.0f

    let calcModifierDendroResPercent modifier =
        match modifier with
        | PercStatModifier sm -> match sm.Type with
                                    | PercStatType.DendroRes -> sm.Value
                                    | _ -> 0.0f
        | _ -> 0.0f