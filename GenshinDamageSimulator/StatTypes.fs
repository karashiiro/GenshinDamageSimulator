namespace GenshinDamageSimulator

[<Struct>]
type FlatStatType = Hp | Attack | Defense | ElementalMastery

[<Struct>]
type PercStatType =
    | Hp | Attack | Defense
    | EnergyRecharge
    | CriticalHit | CriticalDamage
    | Physical | Pyro | Hydro | Electro | Cryo | Anemo | Geo | Dendro
    | PhysicalRes | PyroRes | HydroRes | ElectroRes | CryoRes | AnemoRes | GeoRes | DendroRes

[<Struct>]
type FlatStatModifier =
    { Type: FlatStatType
      Value: uint32 }

[<Struct>]
type PercStatModifier =
    { Type: PercStatType
      Value: float32 }

type StatModifier =
    | FlatStatModifier of FlatStatModifier
    | PercStatModifier of PercStatModifier