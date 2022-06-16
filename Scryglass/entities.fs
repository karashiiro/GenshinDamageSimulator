namespace Scryglass

open Stats
open Time

module Entities =
    type EntityId = uint

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

    type Entity =
        | EnemyEntity of BasicEntityData
        | CharacterEntity of BasicEntityData * CharacterEntityData

    type ActionGroupId = uint
    type InternalCooldown = Seconds * ActionGroupId

    [<RequireQualifiedAccess>]
    type TalentScalingStat = Hp | Attack | Defense
    type Talent =
        { ScalingStat: TalentScalingStat
          ScalingStatMultiplier: float
          ICD: InternalCooldown }