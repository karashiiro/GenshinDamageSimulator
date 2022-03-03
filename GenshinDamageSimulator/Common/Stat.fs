namespace GenshinDamageSimulator

[<RequireQualifiedAccess>]
type BaseStat = Hp | Attack | Defense

[<RequireQualifiedAccess>]
type TalentStat = Hp | Attack | Defense

[<RequireQualifiedAccess>]
type FlatStat = Hp | Attack | Defense | ElementalMastery

[<RequireQualifiedAccess>]
type PercStat =
    | Hp | Attack | Defense
    | EnergyRecharge
    | CriticalHit | CriticalDamage
    | PhysicalBonus | PyroBonus | HydroBonus | ElectroBonus | CryoBonus | AnemoBonus | GeoBonus | DendroBonus
    | PhysicalRes | PyroRes | HydroRes | ElectroRes | CryoRes | AnemoRes | GeoRes | DendroRes

type FlatStatModifier =
    { Type: FlatStat
      Value: float32 }

type PercStatModifier =
    { Type: PercStat
      Value: float32 }

type StatModifier =
    | FlatStatModifier of FlatStatModifier
    | PercStatModifier of PercStatModifier

/// This is the C# interface for stat modifiers.
type StatModifier with
    /// Creates a new flat stat modifier. This method should be preferred over NewFlatStatModifier.
    static member CreateFlat statType value =
        if isNull (box statType) then nullArg "statType"
        FlatStatModifier ({ Type = statType; Value = value })

    /// Creates a new percent stat modifier. This method should be preferred over NewPercStatModifier.
    static member CreatePercent statType value =
        if isNull (box statType) then nullArg "statType"
        PercStatModifier ({ Type = statType; Value = value })