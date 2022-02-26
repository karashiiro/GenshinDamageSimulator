namespace GenshinDamageSimulator.Tests

open FsUnit
open GenshinDamageSimulator
open Xunit

// https://library.keqingmains.com/combat-mechanics/elemental-effects/elemental-gauge-theory

module ElementalAuraStateTests =
    // Helper functions
    let containsPyro = ElementalAuraState.contains Element.Pyro

    let containsHydro = ElementalAuraState.contains Element.Hydro

    let containsElectro = ElementalAuraState.contains Element.Electro

    let containsCryo = ElementalAuraState.contains Element.Cryo

    let containsAnemo = ElementalAuraState.contains Element.Anemo

    let containsGeo = ElementalAuraState.contains Element.Geo

    let containsDendro = ElementalAuraState.contains Element.Dendro

    let getData = ElementalAura.unwrap

    let getCryoData state = state |> ElementalAuraState.get Element.Cryo |> getData

    let createGenericTrigger eu id element =
        { Element = element
          ApplicationSkillId = id
          ApplicationSkillIcdMs = 200f
          Gauge = Gauge.ofUnits eu
          Permanent = false } |> ElementalAura.wrap

    let createPermanentTrigger eu id element =
        element
        |> createGenericTrigger eu id
        |> ElementalAura.unwrap
        |> fun t -> { t with Permanent = true } |> ElementalAura.wrap

    // Universal properties
    let ``has at most two auras`` (input: ElementalAuraState) =
        let state = ElementalAuraState.unwrap input
        state.Count <= 2

    let ``does not have Anemo and Geo together`` (input: ElementalAuraState) =
        input
        |> fun s -> s, s |> containsAnemo
        ||> fun s result -> s |> containsGeo |> (&&) result
        |> not

    let ``does not have Dendro with anything other than Pyro`` (input: ElementalAuraState) =
        if input |> ElementalAuraState.isSingle then
            true
        elif not (input |> containsDendro) then
            true
        else
            input // Has more than one aura, one of which is Dendro
            |> fun s -> s, s |> containsHydro
            ||> fun s result -> s, s |> containsElectro |> (||) result
            ||> fun s result -> s, s |> containsCryo |> (||) result
            ||> fun s result -> s, s |> containsAnemo |> (||) result
            ||> fun s result -> s |> containsGeo |> (||) result
            |> not

    let ``follows universal properties`` (input: ElementalAuraState) =
        input
        |> fun s -> s, ``has at most two auras`` s
        ||> fun s result -> s, result && ``does not have Anemo and Geo together`` s
        ||> fun s result -> result && ``does not have Dendro with anything other than Pyro`` s

    // Tests
    [<Fact>]
    let ``Test that applying an aura on an unaspected state adds the aura to the state without any reaction``() =
        let property s t =
            ElementalAuraState.interact s t
            ||> fun ns r -> ns, ns |> containsCryo && Seq.isEmpty r
            ||> fun ns result -> result && ``follows universal properties`` <| ns
        let state = ElementalAuraState.empty
        let trigger = createGenericTrigger 4f 0u Element.Cryo
        property state trigger |> should be True

    [<Fact>]
    let ``Test that applying a Geo aura on an unaspected state does nothing``() =
        let property s t =
            ElementalAuraState.interact s t
            ||> fun ns r -> ns, ElementalAuraState.isEmpty ns && Seq.isEmpty r
            ||> fun ns result -> result && ``follows universal properties`` <| ns
        let state = ElementalAuraState.empty
        let trigger = createGenericTrigger 4f 0u Element.Geo
        property state trigger |> should be True

    [<Fact>]
    let ``Test that applying an Anemo aura on an unaspected state does nothing``() =
        let property s t =
            ElementalAuraState.interact s t
            ||> fun ns r -> ns, ElementalAuraState.isEmpty ns && Seq.isEmpty r
            ||> fun ns result -> result && ``follows universal properties`` <| ns
        let state = ElementalAuraState.empty
        let trigger = createGenericTrigger 4f 0u Element.Anemo
        property state trigger |> should be True

    [<Fact>]
    let ``Test that applying an aura applies the 20% aura tax``() =
        let property s t =
            ElementalAuraState.interact s t
            ||> fun ns r -> ns, ns |> containsCryo && Seq.isEmpty r
            ||> fun ns result -> ns, result && ns |> ElementalAuraState.get Element.Cryo |> ElementalAura.gauge |> Gauge.eu = 1.6f
            ||> fun ns result -> result && ``follows universal properties`` <| ns
        let state = ElementalAuraState.empty
        let trigger = createGenericTrigger 2f 0u Element.Cryo
        property state trigger |> should be True

    [<Fact>]
    let ``Test that applying an aura on a state that already has that aura will replace it without any reaction``() =
        let property s t1 t2 =
            // First interaction check
            ElementalAuraState.interact s t1
            ||> fun ns r -> ns, ns |> containsCryo && Seq.isEmpty r
            ||> fun ns result -> ns, result && ``follows universal properties`` <| ns
            // Second interaction check
            ||> fun ns result -> ElementalAuraState.interact ns t2, result
            ||> fun (ns, r) result -> ns, result && ns |> containsCryo && Seq.isEmpty r
            ||> fun ns result -> ns, result && ``follows universal properties`` <| ns
            ||> fun ns result -> result && (ns |> getCryoData).ApplicationSkillId = (t2 |> getData).ApplicationSkillId
        let state = ElementalAuraState.empty
        let firstTrigger = createGenericTrigger 4f 0u Element.Cryo
        let secondTrigger = createGenericTrigger 4f 1u Element.Cryo
        property state firstTrigger secondTrigger |> should be True

    [<Fact>]
    let ``Test that applying a non-permanent aura on a state that has a permanent aura will keep the permanent aura``() =
        let property s tp tn =
            // First interaction check
            ElementalAuraState.interact s tp
            ||> fun ns r -> ns, ns |> containsCryo && Seq.isEmpty r
            ||> fun ns result -> ns, result && ``follows universal properties`` <| ns
            // Second interaction check
            ||> fun ns result -> ElementalAuraState.interact ns tn, result
            ||> fun (ns, r) result -> ns, result && ns |> containsCryo && Seq.isEmpty r
            ||> fun ns result -> ns, result && ``follows universal properties`` <| ns
            ||> fun ns result -> result && (ns |> getCryoData).ApplicationSkillId = (tp |> getData).ApplicationSkillId
        let state = ElementalAuraState.empty
        let firstTrigger = createPermanentTrigger 0f 0u Element.Cryo
        let secondTrigger = createGenericTrigger 4f 1u Element.Cryo
        property state firstTrigger secondTrigger |> should be True

    [<Fact>]
    let ``Test that applying a permanent aura on a state that has a non-permanent aura will keep the permanent aura``() =
        let property s tn tp =
            // First interaction check
            ElementalAuraState.interact s tn
            ||> fun ns r -> ns, ns |> containsCryo && Seq.isEmpty r
            ||> fun ns result -> ns, result && ``follows universal properties`` <| ns
            // Second interaction check
            ||> fun ns result -> ElementalAuraState.interact ns tp, result
            ||> fun (ns, r) result -> ns, result && ns |> containsCryo && Seq.isEmpty r
            ||> fun ns result -> ns, result && ``follows universal properties`` <| ns
            ||> fun ns result -> result && (ns |> getCryoData).ApplicationSkillId = (tp |> getData).ApplicationSkillId
        let state = ElementalAuraState.empty
        let firstTrigger = createGenericTrigger 4f 1u Element.Cryo
        let secondTrigger = createPermanentTrigger 0f 0u Element.Cryo
        property state firstTrigger secondTrigger |> should be True

    [<Fact>]
    let ``Test that creating a reaction on a state that has a permanent aura will keep the permanent aura``() =
        let property s tp tn =
            // First interaction check
            ElementalAuraState.interact s tp
            ||> fun ns r -> ns, ns |> containsCryo && Seq.isEmpty r
            ||> fun ns result -> ns, result && ``follows universal properties`` <| ns
            // Second interaction check
            ||> fun ns result -> ElementalAuraState.interact ns tn, result
            ||> fun (ns, r) result -> ns, result && ns |> containsCryo && r |> Seq.contains StrongMelt
            ||> fun ns result -> ns, result && ``follows universal properties`` <| ns
            ||> fun ns result -> result && (ns |> getCryoData).ApplicationSkillId = (tp |> getData).ApplicationSkillId
        let state = ElementalAuraState.empty
        let firstTrigger = createPermanentTrigger 0f 0u Element.Cryo
        let secondTrigger = createGenericTrigger 4f 1u Element.Pyro
        property state firstTrigger secondTrigger |> should be True

    [<Fact>]
    let ``Test that applying a trigger on an empty state sets the gauge correctly``() =
        // "Kaeya's E applies 2B * 0.8 = 1.6B Cryo aura and the decay rate is 7.5s per B."
        let t1 = createGenericTrigger 2f 0u Element.Cryo
        let state = ElementalAuraState.empty
        let state, _ = ElementalAuraState.interact state t1
        let g = state |> ElementalAuraState.get Element.Cryo |> ElementalAura.gauge
        let eu, dr = g |> Gauge.unwrap
        eu |> should be (equal 1.6f)
        dr |> should be (equal 7.5f)

    [<Fact>]
    let ``Test that applying a same-element trigger merges the gauges correctly``() =
        // "Fischl's Charged Shot applies 0.8A Electro, the use of Beidou's Q will add 3.2C
        // Electro to the gauge, resulting in a 3.2A Electro aura persisting for 38 seconds
        // from the time of Beidou Q."
        let t1 = createGenericTrigger 1f 0u Element.Electro
        let t2 = createGenericTrigger 4f 1u Element.Electro
        let state = ElementalAuraState.empty
        let state, _ = ElementalAuraState.interact state t1
        let state, _ = ElementalAuraState.interact state t2
        let g = state |> ElementalAuraState.get Element.Electro |> ElementalAura.gauge
        let eu, dr = g |> Gauge.unwrap
        eu |> should be (equal 3.2f)
        dr |> should be (equal 11.875f)

    [<Fact>]
    let ``Test that creating a 1x transformative reaction subtracts the gauges correctly (Superconduct)``() =
        // "Kaeya's E applies 1.6B Cryo aura and is triggered by Fischl's charged shot,
        // which applies 1A Electro. Superconduct occurs, 0.6B Cryo aura remains."
        let t1 = createGenericTrigger 2f 0u Element.Cryo
        let t2 = createGenericTrigger 1f 1u Element.Electro
        let state = ElementalAuraState.empty
        let state, _ = ElementalAuraState.interact state t1
        let state, reactions = ElementalAuraState.interact state t2
        state |> containsElectro |> should be False
        reactions |> should contain Superconduct
        let g = state |> ElementalAuraState.get Element.Cryo |> ElementalAura.gauge
        let eu, dr = g |> Gauge.unwrap
        eu |> should be (equal 0.6f)
        dr |> should be (equal 7.5f)

    [<Fact>]
    let ``Test that creating a 1x transformative reaction subtracts the gauges correctly (Overload)``() =
        let t1 = createGenericTrigger 2f 0u Element.Pyro
        let t2 = createGenericTrigger 1f 1u Element.Electro
        let state = ElementalAuraState.empty
        let state, _ = ElementalAuraState.interact state t1
        let state, reactions = ElementalAuraState.interact state t2
        state |> containsElectro |> should be False
        reactions |> should contain Overload
        let g = state |> ElementalAuraState.get Element.Pyro |> ElementalAura.gauge
        let eu, dr = g |> Gauge.unwrap
        eu |> should be (equal 0.6f)
        dr |> should be (equal 7.5f)

    [<Fact>]
    let ``Test that creating a 50% modifier transformative reaction subtracts the gauges correctly (Weak Melt)``() =
        // "An enemy affected by Amber's Charged Shot has 1.6B Pyro. Using Kaeya's E (2B Cryo)
        // only removes 1GU Pyro because weak melt occurs when the trigger is Cryo."
        let t1 = createGenericTrigger 2f 0u Element.Pyro
        let t2 = createGenericTrigger 2f 1u Element.Cryo
        let state = ElementalAuraState.empty
        let state, _ = ElementalAuraState.interact state t1
        let state, reactions = ElementalAuraState.interact state t2
        state |> containsCryo |> should be False
        reactions |> should contain WeakMelt
        let g = state |> ElementalAuraState.get Element.Pyro |> ElementalAura.gauge
        let eu, dr = g |> Gauge.unwrap
        eu |> should be (equal 0.6f)
        dr |> should be (equal 7.5f)

    [<Fact>]
    let ``Test that creating a 50% modifier transformative reaction subtracts the gauges correctly (Weak Vaporize)``() =
        let t1 = createGenericTrigger 2f 0u Element.Hydro
        let t2 = createGenericTrigger 2f 1u Element.Pyro
        let state = ElementalAuraState.empty
        let state, _ = ElementalAuraState.interact state t1
        let state, reactions = ElementalAuraState.interact state t2
        state |> containsPyro |> should be False
        reactions |> should contain WeakVaporize
        let g = state |> ElementalAuraState.get Element.Hydro |> ElementalAura.gauge
        let eu, dr = g |> Gauge.unwrap
        eu |> should be (equal 0.6f)
        dr |> should be (equal 7.5f)

    [<Fact>]
    let ``Test that creating a 50% modifier transformative reaction subtracts the gauges correctly (Crystallize)``() =
        let t1 = createGenericTrigger 2f 0u Element.Hydro
        let t2 = createGenericTrigger 2f 1u Element.Geo
        let state = ElementalAuraState.empty
        let state, _ = ElementalAuraState.interact state t1
        let state, reactions = ElementalAuraState.interact state t2
        reactions |> should contain Crystallize
        let g = state |> ElementalAuraState.get Element.Hydro |> ElementalAura.gauge
        let eu, dr = g |> Gauge.unwrap
        eu |> should be (equal 0.6f)
        dr |> should be (equal 7.5f)

    [<Fact>]
    let ``Test that creating a 50% modifier transformative reaction subtracts the gauges correctly (Swirl)``() =
        let t1 = createGenericTrigger 2f 0u Element.Hydro
        let t2 = createGenericTrigger 2f 1u Element.Anemo
        let state = ElementalAuraState.empty
        let state, _ = ElementalAuraState.interact state t1
        let state, reactions = ElementalAuraState.interact state t2
        reactions |> should contain Swirl
        let g = state |> ElementalAuraState.get Element.Hydro |> ElementalAura.gauge
        let eu, dr = g |> Gauge.unwrap
        eu |> should be (equal 0.6f)
        dr |> should be (equal 7.5f)

    [<Fact>]
    let ``Test that creating a 2x transformative reaction subtracts the gauges correctly (Strong Melt)``() =
        // "An enemy affected by Kaeya's E has 1.6B Cryo. Using Diluc's E (1A) removes 2GU worth
        // of Cyro aura because strong melt occurs when the trigger is Pyro. This leaves us with
        // 0GU Cryo as gauges cannot go below zero."
        let t1 = createGenericTrigger 2f 0u Element.Cryo
        let t2 = createGenericTrigger 1f 1u Element.Pyro
        let state = ElementalAuraState.empty
        let state, _ = ElementalAuraState.interact state t1
        let state, reactions = ElementalAuraState.interact state t2
        state |> containsCryo |> should be False
        state |> containsPyro |> should be False
        reactions |> should contain StrongMelt

    [<Fact>]
    let ``Test that creating a 2x transformative reaction subtracts the gauges correctly (Strong Vaporize)``() =
        let t1 = createGenericTrigger 2f 0u Element.Pyro
        let t2 = createGenericTrigger 1f 1u Element.Hydro
        let state = ElementalAuraState.empty
        let state, _ = ElementalAuraState.interact state t1
        let state, reactions = ElementalAuraState.interact state t2
        state |> containsHydro |> should be False
        state |> containsPyro |> should be False
        reactions |> should contain StrongVaporize