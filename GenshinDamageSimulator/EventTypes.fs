namespace GenshinDamageSimulator

[<Struct>]
type ElapseEvent =
    { TimeElapsed: int64<ms> }

[<Struct>]
type CombatantAddEvent = struct end

[<Struct>]
type CombatantRemoveEvent = struct end

[<Struct>]
type PartyAddEvent = struct end

[<Struct>]
type PartyRemoveEvent = struct end

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
    | CombatantRemove of CombatantRemoveEvent
    | PartyAdd of PartyAddEvent
    | PartyRemove of PartyRemoveEvent
    | TalentDamage of TalentDamageEvent
    | TalentHeal of TalentHealEvent

[<Struct>]
type GenesisEventResult = struct end

[<Struct>]
type ElapseEventResult =
    { TimeElapsed: int64<ms> }

[<Struct>]
type CombatantAddEventResult =
    { TargetId: CombatantId }

[<Struct>]
type CombatantRemoveEventResult =
    { TargetId: CombatantId }

[<Struct>]
type PartyAddEventResult =
    { TargetId: CombatantId }

[<Struct>]
type PartyRemoveEventResult =
    { TargetId: CombatantId }

[<Struct>]
type DamageEventResult =
    { TargetId: CombatantId
      DamageAmount: uint32 }

[<Struct>]
type ElementalAuraEventResult =
    { TargetId: CombatantId
      AppliedAura: ElementalAura }

[<Struct>]
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