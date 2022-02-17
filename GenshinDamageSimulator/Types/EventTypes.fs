﻿namespace GenshinDamageSimulator

type ElapseEvent =
    { TimeElapsed: int64<ms> }

type CombatantAddEvent =
    { BNpc: BattleNpc * BattleNpcState }

type CombatantRemoveEvent =
    { TargetId: CombatantId }

type PartyAddEvent =
    { TargetId: CombatantId }

type PartyRemoveEvent =
    { TargetId: CombatantId }

type Critical = AverageCritical | FullCritical | NoCritical

type TalentDamageEvent =
    { DamageType: DamageType
      DamageStat: BaseStat
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

type GenesisEventResult = struct end

type ElapseEventResult =
    { TimeElapsed: int64<ms> }

type CombatantAddEventResult =
    { BNpc: BattleNpc * BattleNpcState }

type CombatantRemoveEventResult =
    { TargetId: CombatantId }

type PartyAddEventResult =
    { TargetId: CombatantId }

type PartyRemoveEventResult =
    { TargetId: CombatantId }

type DamageEventResult =
    { TargetId: CombatantId
      DamageAmount: uint32 }

type ElementalAuraEventResult =
    { TargetId: CombatantId
      AppliedAura: ElementalAura }

type HealEventResult =
    { TargetId: CombatantId
      HealAmount: uint32 }

type GameEventResult =
    | GenesisResult of GenesisEventResult
    | ElapseResult of ElapseEventResult
    | CombatantAddResult of CombatantAddEventResult
    | CombatantRemoveResult of CombatantRemoveEventResult
    | PartyAddResult of PartyAddEventResult
    | PartyRemoveResult of PartyRemoveEventResult
    | DamageResult of DamageEventResult
    | ElementalAuraResult of ElementalAuraEventResult
    | HealResult of HealEventResult