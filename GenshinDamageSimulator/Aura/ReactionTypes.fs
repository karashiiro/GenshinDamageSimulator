namespace GenshinDamageSimulator

type Reaction =
    | StrongVaporize // Pyro <- Hydro
    | Overload
    | WeakMelt // Pyro <- Cryo
    | WeakVaporize // Hydro <- Pyro
    | ElectroCharged
    | Frozen
    | Superconduct
    | StrongMelt // Cryo <- Pyro
    | Swirl
    | Crystallize
    | Burning
    | Shattered // Frozen <- Physical