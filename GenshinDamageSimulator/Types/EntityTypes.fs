﻿namespace GenshinDamageSimulator

type Element = Unknown | Pyro | Hydro | Electro | Cryo | Anemo | Geo | Dendro

type Artifact =
    { MainStat: StatModifier
      StatLines: StatModifier array }

type Weapon =
    { Attack: uint32
      MainStat: StatModifier }

type BattleNpcType = Character | Enemy

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

type AbilityGaugeValue =
    { ElementalGaugeValue: uint8
      DurationValue: float32 }

type ElementalAura =
    { Element: Element
      ApplicationSkillId: uint32 // Used for comparing ICDs
      ApplicationTimeMs: int64<ms> // Nanosecond-precision is also an option, but that doesn't seem meaningful
      Gauge: AbilityGaugeValue }

type CombatantId = uint32

type BattleNpcState =
    { Id: CombatantId
      Hp: uint32
      ShieldHp: uint32
      ElementalAuras: ElementalAura array }