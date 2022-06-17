namespace Scryglass

[<AutoOpen>]
module AuraTypes =
    type AuraData =
        | StandardAura of Gauge * FlatElementalMastery * Level
        | SelfAura of Gauge

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