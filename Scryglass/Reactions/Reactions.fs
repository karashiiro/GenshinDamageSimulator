namespace Scryglass

open Microsoft.FSharp.Data.UnitSystems.SI.UnitNames

[<AutoOpen>]
module Reactions =
    type StrongVaporize = PyroAuraData * HydroAuraData
    type WeakVaporize = HydroAuraData * PyroAuraData
    type StrongMelt = CryoAuraData * PyroAuraData
    type WeakMelt = PyroAuraData * CryoAuraData

    type Overloaded =
        | PyroTriggeredOverloaded of ElectroAuraData * PyroAuraData
        | ElectroTriggeredOverloaded of PyroAuraData * ElectroAuraData
    type Superconduct =
        | CryoTriggeredSuperconduct of ElectroAuraData * CryoAuraData
        | ElectroTriggeredSuperconduct of CryoAuraData * ElectroAuraData
    type ElectroCharged =
        | HydroTriggeredElectroCharged of ElectroAuraData * HydroAuraData
        | ElectroTriggeredElectroCharged of HydroAuraData * ElectroAuraData
    type Frozen =
        | HydroTriggeredFrozen of CryoAuraData * HydroAuraData
        | CryoTriggeredFrozen of HydroAuraData * CryoAuraData
    type Shatter = Frozen * AuraData // Frozen <- Heavy?
    type Swirl = VolatileAura * AnemoAuraData
    type Crystallize = VolatileAura * GeoAuraData
    type Burning =
        | PyroTriggeredBurning of DendroAuraData * PyroAuraData
        | DendroTriggeredBurning of PyroAuraData * DendroAuraData

    [<RequireQualifiedAccess>]
    type Reaction =
        | StrongVaporize of StrongVaporize
        | Overloaded of Overloaded
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
    type Elapse = ElementAuraState -> float<second> -> ElementAuraState