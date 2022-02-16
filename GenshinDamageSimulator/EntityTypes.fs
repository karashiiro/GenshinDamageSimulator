namespace GenshinDamageSimulator

open System

[<Struct>]
type Element = Unknown | Pyro | Hydro | Electro | Cryo | Anemo | Geo | Dendro

[<Struct>]
type Artifact =
    { MainStat: StatModifier
      StatLines: StatModifier array }

[<Struct>]
type Weapon =
    { Attack: uint32
      MainStat: StatModifier }

[<Struct>]
type BattleNpcType = Character | Enemy

[<Struct>]
type BattleNpc =
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
      Level: uint32 // This isn't necessarily fixed, but we'll consider it so for now
      NpcType: BattleNpcType
      MainStat: StatModifier
      Element: Element
      Weapon: Weapon
      Artifacts: Artifact array }

[<Struct>]
type AbilityGaugeValue =
    { ElementalGaugeValue: uint8
      DurationValue: float32 }

[<Struct>]
type ElementalAura =
    { Element: Element
      ApplicationSkillId: uint32 // Used for comparing ICDs
      ApplicationTime: DateTime
      Gauge: AbilityGaugeValue }

[<Struct>]
type BattleNpcState =
    { Id: uint32
      Hp: uint32
      ShieldHp: uint32
      ElementalAuras: ElementalAura array }