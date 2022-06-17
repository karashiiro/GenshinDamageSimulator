namespace Scryglass

[<AutoOpen>]
module DamageTypes =
    type Critical = NeverCritical | AverageCritical | AlwaysCritical

    [<RequireQualifiedAccess>]
    type Infusion = Physical | Pyro | Hydro | Electro | Cryo | Anemo | Geo | Dendro

module Damage =
    let baseDamage talent attacker =
        match talent.ScalingStat with
        | TalentScalingStat.Hp -> Entity.totalHp attacker * talent.ScalingStatMultiplier
        | TalentScalingStat.Attack ->  Entity.totalAttack attacker * talent.ScalingStatMultiplier
        | TalentScalingStat.Defense -> Entity.totalDefense attacker * talent.ScalingStatMultiplier

    let private averageCrit attacker =
        1.0 + (max 0.0 (min 1.0 (Entity.totalCriticalRate attacker))) * Entity.totalCriticalDamage attacker

    let critMult style attacker =
        match style with
        | NeverCritical -> 1.0
        | AverageCritical -> averageCrit attacker
        | AlwaysCritical -> 1.0 + Entity.totalCriticalDamage attacker

    let enemyDefense character enemy =
        let _, data1, _ = character
        let _, data2 = enemy
        let a = data1.Level + 100 |> float
        let b = data2.Level + 100 |> float
        let enemyDefenseReduction = Entity.totalDefenseReduction (enemy |> EnemyEntity)
        let defenseIgnore = Entity.totalDefenseIgnore (character |> CharacterEntity)
        a / (a + b * (1.0 - (min 0.9 enemyDefenseReduction)) * (1.0 - defenseIgnore))

    let resistance infusion attacker defender =
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