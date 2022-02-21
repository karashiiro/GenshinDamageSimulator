namespace GenshinDamageSimulator

type BaseStat = Hp | Attack | Defense

type FlatStat = Hp | Attack | Defense | ElementalMastery

type PercStat =
    | Hp | Attack | Defense
    | EnergyRecharge
    | CriticalHit | CriticalDamage
    | PhysicalBonus | PyroBonus | HydroBonus | ElectroBonus | CryoBonus | AnemoBonus | GeoBonus | DendroBonus
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