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