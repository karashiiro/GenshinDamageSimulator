namespace GenshinDamageSimulator

type Artifact =
    { MainStat: StatModifier
      StatLines: StatModifier array }

type Weapon =
    { Attack: uint32
      MainStat: StatModifier }

type BasicEntityData =
    { BaseHp: uint32
      BaseAttack: uint32
      BaseDefense: uint32
      BasePhysicalRes: float32
      BasePyroRes: float32
      BaseHydroRes: float32
      BaseElectroRes: float32
      BaseCryoRes: float32
      BaseAnemoRes: float32
      BaseGeoRes: float32
      BaseDendroRes: float32
      Level: uint32 } // This isn't necessarily fixed, but we'll consider it so for now

type CharacterEntityData =
    { MainStat: StatModifier
      Element: Element
      Weapon: Weapon
      Artifacts: Artifact array }

type CharacterEntity = BasicEntityData * CharacterEntityData

type Entity =
    | EnemyEntity of BasicEntityData
    | CharacterEntity of CharacterEntity

type EntityId = EntityId of uint32

type EntityState =
    { Id: EntityId
      Hp: uint32
      ShieldHp: uint32
      ElementalAuras: AuraState }