namespace Scryglass

[<AutoOpen>]
module DamageTypes =
    type Critical = NeverCritical | AverageCritical | AlwaysCritical

    [<RequireQualifiedAccess>]
    type Infusion = Physical | Pyro | Hydro | Electro | Cryo | Anemo | Geo | Dendro

module Damage =
    let baseDamage attacker talent =
        match talent.ScalingStat with
        | TalentScalingStat.Hp -> talent.ScalingStatMultiplier * Entity.totalHp attacker
        | TalentScalingStat.Attack -> talent.ScalingStatMultiplier * Entity.totalAttack attacker
        | TalentScalingStat.Defense -> talent.ScalingStatMultiplier * Entity.totalDefense attacker

    let private averageCrit attacker =
        1.0 + (max 0.0 (min 1.0 (Entity.totalCriticalRate attacker))) * Entity.totalCriticalDamage attacker

    let critMult attacker style =
        match style with
        | NeverCritical -> 1.0
        | AverageCritical -> averageCrit attacker
        | AlwaysCritical -> 1.0 + Entity.totalCriticalDamage attacker

    let defenseMult attacker defender =
        let attackerLevelFactor = Entity.level attacker + 100 |> float
        let defenderLevelFactor = Entity.level defender + 100 |> float
        let defenderDefenseReduction = Entity.totalDefenseReduction defender
        let attackerDefenseIgnore = Entity.totalDefenseIgnore attacker
        attackerLevelFactor / (attackerLevelFactor + defenderLevelFactor * (1.0 - (min 0.9 defenderDefenseReduction)) * (1.0 - attackerDefenseIgnore))

    let damageBonus infusion attacker =
        match infusion with
        | Infusion.Physical -> Entity.totalPhysicalBonus attacker
        | Infusion.Pyro -> Entity.totalPyroBonus attacker
        | Infusion.Hydro -> Entity.totalHydroBonus attacker
        | Infusion.Electro -> Entity.totalElectroBonus attacker
        | Infusion.Cryo -> Entity.totalCryoBonus attacker
        | Infusion.Anemo -> Entity.totalAnemoBonus attacker
        | Infusion.Geo -> Entity.totalGeoBonus attacker
        | Infusion.Dendro -> Entity.totalDendroBonus attacker
        |> (+) 1.0

    let resMult infusion attacker defender =
        let baseRes =
            match infusion with
            | Infusion.Physical -> Entity.totalPhysicalRes defender
            | Infusion.Pyro -> Entity.totalPyroRes defender
            | Infusion.Hydro -> Entity.totalHydroRes defender
            | Infusion.Electro -> Entity.totalElectroRes defender
            | Infusion.Cryo -> Entity.totalCryoRes defender
            | Infusion.Anemo -> Entity.totalAnemoRes defender
            | Infusion.Geo -> Entity.totalGeoRes defender
            | Infusion.Dendro -> Entity.totalDendroRes defender
        let resIgnore =
            match infusion with
            | Infusion.Physical -> Entity.totalPhysicalResIgnore attacker
            | Infusion.Pyro -> Entity.totalPyroResIgnore attacker
            | Infusion.Hydro -> Entity.totalHydroResIgnore attacker
            | Infusion.Electro -> Entity.totalElectroResIgnore attacker
            | Infusion.Cryo -> Entity.totalCryoResIgnore attacker
            | Infusion.Anemo -> Entity.totalAnemoResIgnore attacker
            | Infusion.Geo -> Entity.totalGeoResIgnore attacker
            | Infusion.Dendro -> Entity.totalDendroResIgnore attacker
        let res = baseRes - resIgnore
        if res < 0 then
            1.0 - (res / 2.0)
        elif res > 0 && res < 0.75 then
            1.0 - res
        else // res >= 0.75
            1.0 / (4.0 * res + 1.0)

    let flatDamage =
        Entity.totalFlatDamageBonus

    let infusion talent =
        match talent.Aura with
        | Some (Aura.Pyro _) -> Infusion.Pyro
        | Some (Aura.Hydro _) -> Infusion.Hydro
        | Some (Aura.Electro _) -> Infusion.Electro
        | Some (Aura.Cryo _) -> Infusion.Cryo
        | Some (Aura.Anemo _) -> Infusion.Anemo
        | Some (Aura.Geo _) -> Infusion.Geo
        | Some (Aura.Dendro _) -> Infusion.Dendro
        | _ -> Infusion.Physical

    let damage attacker defender talent critical =
        let el = infusion talent
        baseDamage attacker talent
        |> fun d -> d + flatDamage attacker
        |> fun d -> d * damageBonus el attacker
        |> fun d -> d * critMult attacker critical
        |> fun d -> d * defenseMult attacker defender
        |> fun d -> d * resMult el attacker defender