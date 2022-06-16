namespace Scryglass

open Stats
open Time

module Elements =
    type GaugeUnits = float
    type InternalCooldownGroupId = uint
    type InternalCooldown = Seconds * InternalCooldownGroupId
    type AuraData =
        | StandardAura of GaugeUnits * InternalCooldown * FlatElementalMastery * Level // EM and level are snapshot
        | SelfAura of GaugeUnits

    type PyroAuraData = AuraData
    type CryoAuraData = AuraData
    type HydroAuraData = AuraData
    type ElectroAuraData = AuraData
    type AnemoAuraData = AuraData
    type GeoAuraData = AuraData
    type DendroAuraData = AuraData
    
    [<RequireQualifiedAccess>]
    type VolatileAura =
        | Pyro of PyroAuraData
        | Cryo of CryoAuraData
        | Hydro of HydroAuraData
        | Electro of ElectroAuraData

    [<RequireQualifiedAccess>]
    type Aura =
        | Pyro of PyroAuraData
        | Cryo of CryoAuraData
        | Hydro of HydroAuraData
        | Electro of ElectroAuraData
        | Anemo of AnemoAuraData
        | Geo of GeoAuraData
        | Dendro of DendroAuraData

    type ElementAuraState =
        { Pyro: PyroAuraData option
          Cryo: CryoAuraData option
          Hydro: HydroAuraData option
          Electro: ElectroAuraData option
          Anemo: AnemoAuraData option
          Geo: GeoAuraData option
          Dendro: DendroAuraData option }

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