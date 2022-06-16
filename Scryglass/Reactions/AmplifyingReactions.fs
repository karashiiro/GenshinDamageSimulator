namespace Scryglass

open Auras

[<AutoOpen>]
module AmplifyingReactionTypes =
    type StrongVaporize = PyroAuraData * HydroAuraData
    type WeakVaporize = HydroAuraData * PyroAuraData
    type StrongMelt = CryoAuraData * PyroAuraData
    type WeakMelt = PyroAuraData * CryoAuraData

module AmplifyingReactions =
    let amplification initial em bonus =
        initial * (1.0 + (2.78 * em) / (1400.0 + em) + bonus)

module StrongVaporize =
    let create aura trigger =
        (aura, trigger) |> StrongVaporize

    let amplifyDamage ((_, trigger): StrongVaporize) bonus damage =
        match trigger with
        | StandardAura (_, _, em, _) -> damage * AmplifyingReactions.amplification 2.0 em bonus
        | SelfAura _ -> damage

module WeakVaporize =
    let create aura trigger =
        (aura, trigger) |> WeakVaporize

    let amplifyDamage ((_, trigger): WeakVaporize) bonus damage =
        match trigger with
        | StandardAura (_, _, em, _) -> damage * AmplifyingReactions.amplification 1.5 em bonus
        | SelfAura _ -> damage

module StrongMelt =
    let create aura trigger =
        (aura, trigger) |> StrongVaporize

    let amplifyDamage ((_, trigger): StrongVaporize) bonus damage =
        match trigger with
        | StandardAura (_, _, em, _) -> damage * AmplifyingReactions.amplification 2.0 em bonus
        | SelfAura _ -> damage

module WeakMelt =
    let create aura trigger =
        (aura, trigger) |> WeakVaporize

    let amplifyDamage ((_, trigger): WeakVaporize) bonus damage =
        match trigger with
        | StandardAura (_, _, em, _) -> damage * AmplifyingReactions.amplification 1.5 em bonus
        | SelfAura _ -> damage