namespace Scryglass

module Entity =
    type EntityId = uint
    
    [<RequireQualifiedAccess>]
    type TalentStat = Hp | Attack | Defense

    [<RequireQualifiedAccess>]
    type FlatStat = Hp | Attack | Defense | ElementalMastery

    [<RequireQualifiedAccess>]
    type PercentStat =
        | Hp | Attack | Defense
        | EnergyRecharge
        | CriticalHit | CriticalDamage
        | PhysicalBonus | PyroBonus | HydroBonus | ElectroBonus | CryoBonus | AnemoBonus | GeoBonus | DendroBonus
        | PhysicalRes | PyroRes | HydroRes | ElectroRes | CryoRes | AnemoRes | GeoRes | DendroRes
        
    type EntityStat =
        | FlatStat of FlatStat
        | PercentStat of PercentStat
    
    type StatModifier = EntityStat * float
    
    type Artifact =
        { MainStat: StatModifier
          SubStats: StatModifier list }

    type Weapon =
        { Attack: float
          MainStat: StatModifier option }

    type BasicEntityData =
        { BaseHp: float
          BaseAttack: float
          BaseDefense: float
          BasePhysicalRes: float
          BasePyroRes: float
          BaseHydroRes: float
          BaseElectroRes: float
          BaseCryoRes: float
          BaseAnemoRes: float
          BaseGeoRes: float
          BaseDendroRes: float
          Level: uint }

    type CharacterEntityData =
        { MainStat: StatModifier
          CriticalHit: float
          CriticalDamage: float
          EnergyRecharge: float
          Weapon: Weapon
          Artifacts: Artifact list }

    type Entity =
        | EnemyEntity of BasicEntityData
        | CharacterEntity of BasicEntityData * CharacterEntityData