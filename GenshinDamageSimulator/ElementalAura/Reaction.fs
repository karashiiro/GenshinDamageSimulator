﻿namespace GenshinDamageSimulator

type Reaction =
    | StrongVaporize // Pyro <- Hydro
    | Overload
    | WeakMelt // Pyro <- Cryo
    | WeakVaporize // Hydro <- Pyro
    | ElectroCharged of float32 // Tick countdown in seconds
    | Frozen
    | Superconduct
    | StrongMelt // Cryo <- Pyro
    | Swirl
    | Crystallize
    | Burning of float32 // Tick countdown in seconds
    | Shattered // Frozen <- Physical