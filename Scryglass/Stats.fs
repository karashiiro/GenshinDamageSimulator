namespace Scryglass

[<AutoOpen>]
module Stats =
    type StatValue = float

    type Level = int

    type FlatHp = StatValue
    type FlatAttack = StatValue
    type FlatDefense = StatValue
    type FlatElementalMastery = StatValue

    type PercentHp = StatValue
    type PercentAttack = StatValue
    type PercentDefense = StatValue
    type PercentEnergyRecharge = StatValue
    type PercentCriticalRate = StatValue
    type PercentCriticalDamage = StatValue
    type PercentPhysicalBonus = StatValue
    type PercentPyroBonus = StatValue
    type PercentHydroBonus = StatValue
    type PercentElectroBonus = StatValue
    type PercentCryoBonus = StatValue
    type PercentAnemoBonus = StatValue
    type PercentGeoBonus = StatValue
    type PercentDendroBonus = StatValue
    type PercentPhysicalRes = StatValue
    type PercentPyroRes = StatValue
    type PercentHydroRes = StatValue
    type PercentElectroRes = StatValue
    type PercentCryoRes = StatValue
    type PercentAnemoRes = StatValue
    type PercentGeoRes = StatValue
    type PercentDendroRes = StatValue

    type StatModifier =
        | FlatHp of FlatHp
        | FlatAttack of FlatAttack
        | FlatDefense of FlatDefense
        | FlatElementalMastery of FlatElementalMastery
        | PercentHp of PercentHp
        | PercentAttack of PercentAttack
        | PercentDefense of PercentDefense
        | PercentEnergyRecharge of PercentEnergyRecharge
        | PercentCriticalRate of PercentCriticalRate
        | PercentCriticalDamage of PercentCriticalDamage
        | PercentPhysicalBonus of PercentPhysicalBonus
        | PercentPyroBonus of PercentPyroBonus
        | PercentHydroBonus of FlatAttack
        | PercentElectroBonus of PercentElectroBonus
        | PercentCryoBonus of PercentCryoBonus
        | PercentAnemoBonus of PercentAnemoBonus
        | PercentGeoBonus of PercentGeoBonus
        | PercentDendroBonus of PercentDendroBonus
        | PercentPhysicalRes of PercentPhysicalRes
        | PercentPyroRes of PercentPyroRes
        | PercentHydroRes of FlatAttack
        | PercentElectroRes of PercentElectroRes
        | PercentCryoRes of PercentCryoRes
        | PercentAnemoRes of PercentAnemoRes
        | PercentGeoRes of PercentGeoRes
        | PercentDendroRes of PercentDendroRes