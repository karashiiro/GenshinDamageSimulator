namespace GenshinDamageSimulator

[<Struct>]
type ElapseEvent =
    { TimeElapsed: int64<ms> }

[<Struct>]
type CombatantAddEvent =
    { TargetId: uint32 }

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
    | Elapse of ElapseEvent
    | CombatantAdd of CombatantAddEvent
    | TalentDamage of TalentDamageEvent
    | TalentHeal of TalentHealEvent

[<Struct>]
type ElapseEventResult =
    { TimeElapsed: int64<ms> }

[<Struct>]
type CombatantAddEventResult =
    { TargetId: uint32 }

[<Struct>]
type DamageEventResult =
    { TargetId: uint32
      DamageAmount: uint32 }

[<Struct>]
type ElementalAuraEventResult =
    { TargetId: uint32
      AppliedAura: ElementalAura }

[<Struct>]
type HealEventResult =
    { TargetId: uint32
      HealAmount: uint32 }

type GameEventResult =
    | ElapseResult of ElapseEventResult
    | CombatantAddResult of CombatantAddEventResult
    | DamageResult of DamageEventResult
    | ElementalAuraResult of ElementalAuraEventResult
    | HealResult of HealEventResult