namespace GenshinDamageSimulator

type BaseStat = Hp | Attack | Defense

type FlatStat = Hp | Attack | Defense | ElementalMastery

type PercStat =
    | Hp | Attack | Defense
    | EnergyRecharge
    | CriticalHit | CriticalDamage
    | Physical | Pyro | Hydro | Electro | Cryo | Anemo | Geo | Dendro
    | PhysicalRes | PyroRes | HydroRes | ElectroRes | CryoRes | AnemoRes | GeoRes | DendroRes

type FlatStatModifier =
    { Type: FlatStat
      Value: uint32 }

type PercStatModifier =
    { Type: PercStat
      Value: float32 }

type StatModifier =
    | FlatStatModifier of FlatStatModifier
    | PercStatModifier of PercStatModifier