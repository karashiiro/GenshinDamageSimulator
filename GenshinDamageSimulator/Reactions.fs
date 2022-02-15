namespace GenshinDamageSimulator

module Reactions =
    let getReaction firstElement secondElement =
        match firstElement with
            | Pyro -> match secondElement with
                            | Hydro -> Some(Vaporize)
                            | Electro -> Some(Overloaded)
                            | Cryo -> Some(ReverseMelt)
                            | Anemo -> Some(Swirl)
                            | Geo -> Some(Crystallize)
                            | Dendro -> Some(Burning)
                            | _ -> None
            | Hydro -> match secondElement with
                            | Pyro -> Some(ReverseVaporize)
                            | Electro -> Some(ElectroCharged)
                            | Cryo -> Some(Frozen)
                            | Anemo -> Some(Swirl)
                            | Geo -> Some(Crystallize)
                            | _ -> None
            | Electro -> match secondElement with
                            | Pyro -> Some(Overloaded)
                            | Hydro -> Some(ElectroCharged)
                            | Cryo -> Some(Superconduct)
                            | Anemo -> Some(Swirl)
                            | Geo -> Some(Crystallize)
                            | _ -> None
            | Cryo -> match secondElement with
                            | Pyro -> Some(Melt)
                            | Hydro -> Some(Frozen)
                            | Electro -> Some(Superconduct)
                            | Anemo -> Some(Swirl)
                            | Geo -> Some(Crystallize)
                            | _ -> None
            | Anemo -> match secondElement with
                            | Anemo -> None
                            | Geo -> None
                            | Dendro -> None
                            | _ -> Some(Swirl)
            | Geo -> match secondElement with
                            | Anemo -> None
                            | Geo -> None
                            | Dendro -> None
                            | _ -> Some(Crystallize)
            | Dendro -> match secondElement with
                            | Pyro -> Some(Burning)
                            | _ -> None
            | _ -> None

    let getAmpifyingReactionMultiplier reaction =
        match reaction with
            | Vaporize -> 2f
            | ReverseVaporize -> 1.5f
            | Melt -> 2f
            | ReverseMelt -> 1.5f
            | _ -> 1f

    let getTransformativeReactionMultiplier reaction =
        match reaction with
            | Superconduct -> 1f
            | Swirl -> 1.2f
            | ElectroCharged -> 2.4f
            | Shattered -> 3f
            | Overloaded -> 4f
            | _ -> 0f