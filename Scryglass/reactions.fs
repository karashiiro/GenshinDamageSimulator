namespace Scryglass

module Reactions =
    open Auras
    open Time

    type StrongVaporize = PyroAuraData * HydroAuraData
    type WeakVaporize = HydroAuraData * PyroAuraData
    type StrongMelt = CryoAuraData * PyroAuraData
    type WeakMelt = PyroAuraData * CryoAuraData

    type Overload = PyroAuraData * ElectroAuraData
    type Superconduct = ElectroAuraData * CryoAuraData
    type ElectroCharged = ElectroAuraData * HydroAuraData
    type Shatter = CryoAuraData  // Frozen <- Heavy
    type Frozen = HydroAuraData * CryoAuraData
    type Swirl = VolatileAura * AnemoAuraData
    type Crystallize = VolatileAura * GeoAuraData
    type Burning = PyroAuraData * DendroAuraData

    type Reaction =
        | StrongVaporize of StrongVaporize
        | Overload of Overload
        | WeakMelt of WeakMelt
        | WeakVaporize of WeakVaporize
        | ElectroCharged of ElectroCharged
        | Frozen of Frozen
        | Shatter of Shatter
        | Superconduct of Superconduct
        | StrongMelt of StrongMelt
        | Swirl of Swirl
        | Crystallize of Crystallize
        | Burning of Burning

    type ApplyAura = ElementAuraState -> Aura -> ElementAuraState * Reaction option
    type Elapse = ElementAuraState -> Seconds -> ElementAuraState