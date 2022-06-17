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