namespace GenshinDamageSimulator

open System

type OriginEventResult() = class end

type ElapseEventResult =
    { TimeElapsedMs: int64 }

type CombatantAddEventResult =
    { Entity: Entity
      EntityState: EntityState }

type CombatantRemoveEventResult =
    { TargetId: EntityId }

type PartyAddEventResult =
    { TargetId: EntityId }

type PartyRemoveEventResult =
    { TargetId: EntityId }

type DamageEventResult =
    { TargetId: EntityId
      DamageAmount: float32
      DamageAura: ElementalAura option }

type ElementalAuraEventResult =
    { TargetId: EntityId
      AppliedAura: ElementalAura }

type HealEventResult =
    { TargetId: EntityId
      HealAmount: float32 }

type GameEventResult =
    | OriginResult of OriginEventResult
    | ElapseResult of ElapseEventResult
    | CombatantAddResult of CombatantAddEventResult
    | CombatantRemoveResult of CombatantRemoveEventResult
    | PartyAddResult of PartyAddEventResult
    | PartyRemoveResult of PartyRemoveEventResult
    | DamageResult of DamageEventResult
    | ElementalAuraResult of ElementalAuraEventResult
    | HealResult of HealEventResult