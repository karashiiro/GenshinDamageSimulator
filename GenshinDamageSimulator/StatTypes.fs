namespace GenshinDamageSimulator

[<Struct>]
type BaseStat = Hp | Attack | Defense

[<Struct>]
type FlatStat = Hp | Attack | Defense | ElementalMastery

[<Struct>]
type PercStat =
    | Hp | Attack | Defense
    | EnergyRecharge
    | CriticalHit | CriticalDamage
    | Physical | Pyro | Hydro | Electro | Cryo | Anemo | Geo | Dendro
    | PhysicalRes | PyroRes | HydroRes | ElectroRes | CryoRes | AnemoRes | GeoRes | DendroRes

[<Struct>]
type FlatStatModifier =
    { Type: FlatStat
      Value: uint32 }

[<Struct>]
type PercStatModifier =
    { Type: PercStat
      Value: float32 }

type StatModifier =
    | FlatStatModifier of FlatStatModifier
    | PercStatModifier of PercStatModifier