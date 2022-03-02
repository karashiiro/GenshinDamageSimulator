﻿namespace GenshinDamageSimulator

type ElapseEvent =
    { TimeElapsedMs: int64 }

type CombatantAddEvent =
    { Entity: Entity
      EntityState: EntityState }

type CombatantRemoveEvent() = class end

type PartyAddEvent() = class end

type PartyRemoveEvent() = class end

type Critical = AverageCritical | FullCritical | NoCritical

type TalentDamageEvent =
    { DamageType: DamageType
      DamageStat: TalentStat
      DamageStatMultiplier: float32
      Critical: Critical }

type TalentHealEvent =
    { HealStat: BaseStat
      HealStatMultiplier: float32 }

type GameEvent =
    | Elapse of ElapseEvent
    | CombatantAdd of CombatantAddEvent
    | CombatantRemove of CombatantRemoveEvent
    | PartyAdd of PartyAddEvent
    | PartyRemove of PartyRemoveEvent
    | TalentDamage of TalentDamageEvent
    | TalentHeal of TalentHealEvent