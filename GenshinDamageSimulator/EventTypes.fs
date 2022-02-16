namespace GenshinDamageSimulator

[<Struct>]
type Critical = AverageCritical | FullCritical | NoCritical

[<Struct>]
type TalentDamageEvent =
    { DamageType: DamageType
      DamageStat: BaseStat
      DamageStatMultiplier: float32
      Critical: Critical }

[<Struct>]
type TalentHealEvent =
    { HealStat: BaseStat
      HealStatMultiplier: float32 }

type GameEvent =
    | TalentDamage of TalentDamageEvent
    | TalentHeal of TalentHealEvent

[<Struct>]
type DamageEventResult =
    { DamageAmount: uint32 }

[<Struct>]
type ElementalAuraEventResult =
    { AppliedAura: ElementalAura }

[<Struct>]
type HealEventResult =
    { HealAmount: uint32 }

type GameEventResult =
    | DamageResult of DamageEventResult
    | ElementalAuraResult of ElementalAuraEventResult
    | HealResult of HealEventResult