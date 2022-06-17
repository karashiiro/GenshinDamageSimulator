namespace Scryglass

module TransformativeReactions =
    let calcDamage calcLevelMult initial em bonus level =
        initial * (1.0 + (16.0 * em) / (2000.0 + em) + bonus) * (calcLevelMult level)

    let calcDamageTrigger initial trigger calcLevelMult bonus =
        match trigger with
        | StandardAura(_, em, lv) -> calcDamage calcLevelMult initial em bonus lv
        | SelfAura _ -> 0.0

module Overloaded =
    let create aura trigger =
        match (aura, trigger) with
        | Aura.Pyro pyro, Aura.Electro electro -> (pyro, electro) |> ElectroTriggeredOverloaded |> Some
        | Aura.Electro electro, Aura.Pyro pyro -> (electro, pyro) |> PyroTriggeredOverloaded |> Some
        | _ -> None

    let calcDamage overloaded =
        match overloaded with
        | PyroTriggeredOverloaded (_, pyro) -> TransformativeReactions.calcDamageTrigger 4.0 pyro
        | ElectroTriggeredOverloaded (_, electro) -> TransformativeReactions.calcDamageTrigger 4.0 electro

module Superconduct =
    let create aura trigger =
        match (aura, trigger) with
        | Aura.Cryo cryo, Aura.Electro electro -> (cryo, electro) |> ElectroTriggeredSuperconduct |> Some
        | Aura.Electro electro, Aura.Cryo cryo -> (electro, cryo) |> CryoTriggeredSuperconduct |> Some
        | _ -> None

    let calcDamage superconduct =
        match superconduct with
        | CryoTriggeredSuperconduct (_, cryo) -> TransformativeReactions.calcDamageTrigger 1.0 cryo
        | ElectroTriggeredSuperconduct (_, electro) -> TransformativeReactions.calcDamageTrigger 1.0 electro

module ElectroCharged =
    let create aura trigger =
        match (aura, trigger) with
        | Aura.Hydro hydro, Aura.Electro electro -> (hydro, electro) |> ElectroTriggeredElectroCharged |> Some
        | Aura.Electro electro, Aura.Hydro hydro -> (electro, hydro) |> HydroTriggeredElectroCharged |> Some
        | _ -> None

    let calcDamage electroCharged =
        match electroCharged with
        | HydroTriggeredElectroCharged (_, hydro) -> TransformativeReactions.calcDamageTrigger 2.4 hydro
        | ElectroTriggeredElectroCharged (_, electro) -> TransformativeReactions.calcDamageTrigger 2.4 electro

module Frozen =
    let create aura trigger =
        match (aura, trigger) with
        | Aura.Hydro hydro, Aura.Cryo cryo -> (hydro, cryo) |> CryoTriggeredFrozen |> Some
        | Aura.Cryo cryo, Aura.Hydro hydro -> (cryo, hydro) |> HydroTriggeredFrozen |> Some
        | _ -> None

    let calcDamage frozen =
        match frozen with
        | CryoTriggeredFrozen _ -> 0.0
        | HydroTriggeredFrozen _ -> 0.0

module Shatter =
    let create reaction trigger =
        match reaction with
        | Reaction.Frozen f -> (f, trigger) |> Shatter |> Some
        | _ -> None

    let calcDamage (shatter: Shatter) =
        let _, trigger = shatter
        TransformativeReactions.calcDamageTrigger 3.0 trigger

module Swirl =
    let create aura trigger =
        (aura, trigger) |> Swirl

    let calcDamage (swirl: Swirl) calcLevelMult bonus =
        let _, trigger = swirl
        TransformativeReactions.calcDamageTrigger 1.2 trigger calcLevelMult bonus

module Crystallize =
    let create aura trigger =
        (aura, trigger) |> Crystallize

module Burning =
    let create aura trigger =
        match (aura, trigger) with
        | Aura.Hydro hydro, Aura.Electro electro -> (hydro, electro) |> ElectroTriggeredElectroCharged |> Some
        | Aura.Electro electro, Aura.Hydro hydro -> (electro, hydro) |> HydroTriggeredElectroCharged |> Some
        | _ -> None

    let calcDamage burning =
        match burning with // 0.25?
        | PyroTriggeredBurning (_, pyro) -> TransformativeReactions.calcDamageTrigger 0.25 pyro
        | DendroTriggeredBurning (_, dendro) -> TransformativeReactions.calcDamageTrigger 0.25 dendro