namespace Scryglass

module Elements =
    type GaugeUnits = float
    type InternalCooldown = float
    type Aura =
        | StandardAura of GaugeUnits * InternalCooldown * Entities.EntityId
        | SelfAura of GaugeUnits
    
    type Element = Pyro | Cryo | Hydro | Electro | Anemo | Geo | Dendro
    type ElementAuraState = Map<Element, Aura>
    
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
    
    type Physical = InternalCooldown * Entities.EntityId
    
    type ApplyAura = ElementAuraState -> Aura -> ElementAuraState * Reaction option
    type ApplyPhysical = ElementAuraState -> Physical -> ElementAuraState * Reaction option
    type Elapse = ElementAuraState -> Time.Seconds -> ElementAuraState