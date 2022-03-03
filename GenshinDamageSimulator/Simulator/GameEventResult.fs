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

/// This is the C# interface for game event result objects.
type GameEventResult with
    // TODO: This is gross
    member this.Match<'T>
        (handleOrigin: Func<OriginEventResult, 'T>)
        (handleElapse: Func<ElapseEventResult, 'T>)
        (handleCombatantAdd: Func<CombatantAddEventResult, 'T>)
        (handleCombatantRemove: Func<CombatantRemoveEventResult, 'T>)
        (handlePartyAdd: Func<PartyAddEventResult, 'T>)
        (handlePartyRemove: Func<PartyRemoveEventResult, 'T>)
        (handleDamage: Func<DamageEventResult, 'T>)
        (handleElementalAura: Func<ElementalAuraEventResult, 'T>)
        (handleHeal: Func<HealEventResult, 'T>)=
        match this with
        | OriginResult r -> handleOrigin.Invoke(r)
        | ElapseResult r -> handleElapse.Invoke(r)
        | CombatantAddResult r -> handleCombatantAdd.Invoke(r)
        | CombatantRemoveResult r -> handleCombatantRemove.Invoke(r)
        | PartyAddResult r -> handlePartyAdd.Invoke(r)
        | PartyRemoveResult r -> handlePartyRemove.Invoke(r)
        | DamageResult r -> handleDamage.Invoke(r)
        | ElementalAuraResult r -> handleElementalAura.Invoke(r)
        | HealResult r -> handleHeal.Invoke(r)