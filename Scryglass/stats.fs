namespace Scryglass

module Stats =
    type StatDataType = float

    type Level = uint

    type FlatHp = StatDataType
    type FlatAttack = StatDataType
    type FlatDefense = StatDataType
    type FlatElementalMastery = StatDataType

    type PercentHp = StatDataType
    type PercentAttack = StatDataType
    type PercentDefense = StatDataType
    type PercentEnergyRecharge = StatDataType
    type PercentCriticalHit = StatDataType
    type PercentCriticalDamage = StatDataType
    type PercentPhysicalBonus = StatDataType
    type PercentPyroBonus = StatDataType
    type PercentHydroBonus = StatDataType
    type PercentElectroBonus = StatDataType
    type PercentCryoBonus = StatDataType
    type PercentAnemoBonus = StatDataType
    type PercentGeoBonus = StatDataType
    type PercentDendroBonus = StatDataType
    type PercentPhysicalRes = StatDataType
    type PercentPyroRes = StatDataType
    type PercentHydroRes = StatDataType
    type PercentElectroRes = StatDataType
    type PercentCryoRes = StatDataType
    type PercentAnemoRes = StatDataType
    type PercentGeoRes = StatDataType
    type PercentDendroRes = StatDataType

    [<RequireQualifiedAccess>]
    type TalentScalingStat = Hp | Attack | Defense

    type StatModifier =
        | FlatHp of FlatHp
        | FlatAttack of FlatAttack
        | FlatDefense of FlatDefense
        | FlatElementalMastery of FlatElementalMastery
        | PercentHp of PercentHp
        | PercentAttack of PercentAttack
        | PercentDefense of PercentDefense
        | PercentEnergyRecharge of PercentEnergyRecharge
        | PercentCriticalHit of PercentCriticalHit
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