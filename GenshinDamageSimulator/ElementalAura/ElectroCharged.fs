namespace GenshinDamageSimulator

type ElectroCharged = ElectroCharged of float32 * uint32

module ElectroCharged =
    /// Wraps the provided values in an Electro-Charged reaction.    
    let wrap t em =
        (t, em) |> ElectroCharged

    /// Unwraps the provided Electro-Charged reaction into its constituent values.
    let unwrap ec =
        let (ElectroCharged (t, em)) = ec
        t, em

    /// Resets the cooldown for the provided Electro-Charged reaction.
    let reset ec =
        ec |> unwrap ||> (fun _ em -> 1f, em) ||> wrap

    /// Sets the cooldown for the provided Electro-Charged reaction.
    let cooldown t ec =
        ec |> unwrap ||> (fun _ em -> t, em) ||> wrap

    /// Updates the Elemental Mastery for the provided Electro-Charged reaction.
    let snapshot em ec =
        ec |> unwrap ||> (fun t _ -> t, em) ||> wrap