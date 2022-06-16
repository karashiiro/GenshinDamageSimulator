namespace Scryglass

module Entities =
    type EntityId = uint
    
    type StatValue = float

    type TalentStat = Hp | Attack | Defense

    type FlatStat = Hp | Attack | Defense | ElementalMastery

    type PercentStat =
        | Hp | Attack | Defense
        | EnergyRecharge
        | CriticalHit | CriticalDamage
        | PhysicalBonus | PyroBonus | HydroBonus | ElectroBonus | CryoBonus | AnemoBonus | GeoBonus | DendroBonus
        | PhysicalRes | PyroRes | HydroRes | ElectroRes | CryoRes | AnemoRes | GeoRes | DendroRes

    type EntityStat =
        | FlatStat of FlatStat
        | PercentStat of PercentStat

    type StatModifier = EntityStat * StatValue

    type Artifact =
        { MainStat: StatModifier
          SubStats: StatModifier list }

    type Weapon =
        { Attack: float
          MainStat: StatModifier option }

    type BasicEntityData =
        { BaseHp: StatValue
          BaseAttack: StatValue
          BaseDefense: StatValue
          BasePhysicalRes: StatValue
          BasePyroRes: StatValue
          BaseHydroRes: StatValue
          BaseElectroRes: StatValue
          BaseCryoRes: StatValue
          BaseAnemoRes: StatValue
          BaseGeoRes: StatValue
          BaseDendroRes: StatValue
          Level: uint }

    type CharacterEntityData =
        { MainStat: StatModifier
          CriticalHit: StatValue
          CriticalDamage: StatValue
          EnergyRecharge: StatValue
          Weapon: Weapon
          Artifacts: Artifact list }

    type Entity =
        | EnemyEntity of BasicEntityData
        | CharacterEntity of BasicEntityData * CharacterEntityData