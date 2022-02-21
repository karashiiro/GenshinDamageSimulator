namespace GenshinDamageSimulator

// https://docs.google.com/document/d/e/2PACX-1vSEovpheHaeum4Ba0MlNdfyOTsJ-wZzDmBof13bVztYtKDi6OQhLqNdwEkEApo6vvtAV0L_tMal2ZTN/pub#h.6crtulvx1dlt

type GaugeUnits = GaugeUnits of float32

type AuraData =
    { Element: Element
      ApplicationSkillId: uint32 // Used for comparing ICDs
      ApplicationSkillIcdMs: float32
      GaugeUnits: GaugeUnits
      Permanent: bool }

type Aura =
    | PyroAura of AuraData
    | HydroAura of AuraData
    | ElectroAura of AuraData
    | CryoAura of AuraData
    | AnemoAura of AuraData
    | GeoAura of AuraData
    | DendroAura of AuraData

type AuraState = Map<Element, Aura>