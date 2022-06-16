namespace Scryglass

open Entities
open Stats
open Time

module Elements =
    type GaugeUnits = float
    type InternalCooldown = Seconds
    type Aura =
        | StandardAura of GaugeUnits * InternalCooldown * EntityId
        | SelfAura of GaugeUnits

    type Element = Pyro | Cryo | Hydro | Electro | Anemo | Geo | Dendro
    type ElementAuraState = Map<Element, Aura>

    type StrongVaporize = FlatElementalMastery

    type Reaction =
        | StrongVaporize // Pyro <- Hydro
        | Overload
        | WeakMelt // Pyro <- Cryo
        | WeakVaporize // Hydro <- Pyro
        | ElectroCharged
        | Frozen
        | Shatter // Frozen <- Physical
        | Superconduct
        | StrongMelt // Cryo <- Pyro
        | Swirl
        | Crystallize
        | Burning

    type Physical = InternalCooldown * EntityId

    type ApplyAura = ElementAuraState -> Aura -> ElementAuraState * Reaction option
    type ApplyPhysical = ElementAuraState -> Physical -> ElementAuraState * Reaction option
    type Elapse = ElementAuraState -> Time.Seconds -> ElementAuraState