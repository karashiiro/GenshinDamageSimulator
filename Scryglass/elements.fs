namespace Scryglass

module Elements =
    type Entity = uint32
    
    type GaugeUnits = float
    type InternalCooldown = float
    type Aura =
        | StandardAura of GaugeUnits * InternalCooldown * Entity
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
    
    type Physical = InternalCooldown * Entity
    
    type ApplyAura = ElementAuraState -> Aura -> ElementAuraState * Reaction option
    type ApplyPhysical = ElementAuraState -> Physical -> ElementAuraState * Reaction option
    type Elapse = ElementAuraState -> Time.Seconds -> ElementAuraState