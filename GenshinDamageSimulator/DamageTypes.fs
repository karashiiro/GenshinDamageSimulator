namespace GenshinDamageSimulator

[<Struct>]
type DamageType = Physical | Pyro | Hydro | Electro | Cryo | Anemo | Geo | Dendro

[<Struct>]
type ElementalReaction =
    | Vaporize // Pyro -> Hydro
    | Overloaded
    | ReverseMelt // Pyro -> Cryo
    | ReverseVaporize // Hydro -> Pyro
    | ElectroCharged
    | Frozen
    | Superconduct
    | Melt // Cryo -> Pyro
    | Swirl
    | Crystallize
    | Burning
    | Shattered // Frozen -> Physical