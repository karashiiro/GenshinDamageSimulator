namespace Scryglass

[<AutoOpen>]
module DamageTypes =
    type Critical = NeverCritical | AverageCritical | AlwaysCritical

    [<RequireQualifiedAccess>]
    type Infusion = Physical | Pyro | Hydro | Electro | Cryo | Anemo | Geo | Dendro

module Damage =
    let baseDamage talent entity =
        match talent.ScalingStat with
        | TalentScalingStat.Hp -> Entity.totalHp entity * talent.ScalingStatMultiplier
        | TalentScalingStat.Attack ->  Entity.totalAttack entity * talent.ScalingStatMultiplier
        | TalentScalingStat.Defense -> Entity.totalDefense entity * talent.ScalingStatMultiplier

    let private averageCrit entity =
        1.0 + (max 0.0 (min 1.0 (Entity.totalCriticalRate entity))) * Entity.totalCriticalDamage entity

    let critMult style entity =
        match style with
        | NeverCritical -> 1.0
        | AverageCritical -> averageCrit entity
        | AlwaysCritical -> 1.0 + Entity.totalCriticalDamage entity

    let enemyDefense character enemy =
        let _, data1, _ = character
        let _, data2 = enemy
        let a = data1.Level + 100 |> float
        let b = data2.Level + 100 |> float
        let enemyDefenseReduction = Entity.totalDefenseReduction (enemy |> EnemyEntity)
        let defenseIgnore = Entity.totalDefenseIgnore (character |> CharacterEntity)
        a / (a + b * (1.0 - (min 0.9 enemyDefenseReduction)) * (1.0 - defenseIgnore))