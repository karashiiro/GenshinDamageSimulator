namespace Scryglass

open Microsoft.FSharp.Data.UnitSystems.SI.UnitNames

[<AutoOpen>]
module EntityTypes =
    type Artifact =
        { MainStat: StatModifier
          SubStats: StatModifier list }

    type Weapon =
        { Attack: FlatAttack
          MainStat: StatModifier option }

    type BasicEntityData =
        { BaseHp: FlatHp
          BaseAttack: FlatAttack
          BaseDefense: FlatDefense
          BaseElementalMastery: FlatElementalMastery
          BasePhysicalRes: PercentPhysicalRes
          BasePyroRes: PercentPyroRes
          BaseHydroRes: PercentHydroRes
          BaseElectroRes: PercentElectroRes
          BaseCryoRes: PercentCryoRes
          BaseAnemoRes: PercentAnemoRes
          BaseGeoRes: PercentGeoRes
          BaseDendroRes: PercentDendroRes
          Level: Level }

    type CharacterEntityData =
        { MainStat: StatModifier
          BaseCriticalHit: PercentCriticalHit
          BaseCriticalDamage: PercentCriticalDamage
          BaseEnergyRecharge: PercentEnergyRecharge
          Weapon: Weapon
          Artifacts: Artifact list }

    type InternalCooldownGroupId = uint
    type InternalCooldown = InternalCooldownGroupId * float<second>

    type Shield =
        { CurrentHp: StatValue
          Strength: StatValue }

    type EntityState =
        { CurrentHp: StatValue
          InternalCooldowns: InternalCooldown list }

    type Entity =
        | EnemyEntity of EntityState * BasicEntityData
        | CharacterEntity of EntityState * BasicEntityData * CharacterEntityData

    [<RequireQualifiedAccess>]
    type TalentScalingStat = Hp | Attack | Defense
    type Talent =
        { ScalingStat: TalentScalingStat
          ScalingStatMultiplier: float
          Aura: Aura option }