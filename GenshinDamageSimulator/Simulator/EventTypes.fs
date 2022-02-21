namespace GenshinDamageSimulator

type ElapseEvent =
    { TimeElapsedMs: int64 }

type CombatantAddEvent =
    { BNpc: Entity
      BNpcState: EntityState }

type CombatantRemoveEvent() = class end

type PartyAddEvent() = class end

type PartyRemoveEvent() = class end

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

type GenesisEventResult() = class end

type ElapseEventResult =
    { TimeElapsedMs: int64 }

type CombatantAddEventResult =
    { BNpc: Entity
      BNpcState: EntityState }

type CombatantRemoveEventResult =
    { TargetId: EntityId }

type PartyAddEventResult =
    { TargetId: EntityId }

type PartyRemoveEventResult =
    { TargetId: EntityId }

type DamageEventResult =
    { TargetId: EntityId
      DamageAmount: uint32
      DamageAura: Aura option }

type ElementalAuraEventResult =
    { TargetId: EntityId
      AppliedAura: Aura }

type HealEventResult =
    { TargetId: EntityId
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