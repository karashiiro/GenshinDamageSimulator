namespace Scryglass

module AmplifyingReactions =
    let amplification initial em bonus =
        initial * (1.0 + (2.78 * em) / (1400.0 + em) + bonus)

    let amplifyDamage initial (_, trigger) bonus damage =
        match trigger with
        | StandardAura (_, em, _) -> damage * amplification initial em bonus
        | SelfAura _ -> damage

module StrongVaporize =
    let create aura trigger =
        (aura, trigger) |> StrongVaporize

    let amplifyDamage (reaction: StrongVaporize) =
        AmplifyingReactions.amplifyDamage 2.0 reaction

module WeakVaporize =
    let create aura trigger =
        (aura, trigger) |> WeakVaporize

    let amplifyDamage (reaction: WeakVaporize) =
        AmplifyingReactions.amplifyDamage 1.5 reaction

module StrongMelt =
    let create aura trigger =
        (aura, trigger) |> StrongVaporize

    let amplifyDamage (reaction: StrongVaporize) =
        AmplifyingReactions.amplifyDamage 2.0 reaction

module WeakMelt =
    let create aura trigger =
        (aura, trigger) |> WeakVaporize

    let amplifyDamage (reaction: WeakVaporize) =
        AmplifyingReactions.amplifyDamage 1.5 reaction