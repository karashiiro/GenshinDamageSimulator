namespace GenshinDamageSimulator.Tests

open FsUnit
open GenshinDamageSimulator
open Xunit

module ElementalAuraStateTests =
    // Helper functions
    let containsHydro = ElementalAuraState.contains Element.Hydro

    let containsElectro = ElementalAuraState.contains Element.Electro

    let containsCryo = ElementalAuraState.contains Element.Cryo

    let containsAnemo = ElementalAuraState.contains Element.Anemo

    let containsGeo = ElementalAuraState.contains Element.Geo

    let containsDendro = ElementalAuraState.contains Element.Dendro

    let getData = ElementalAura.unwrap

    let getCryoData state = state |> ElementalAuraState.get Element.Cryo |> getData

    let createGenericTrigger gu id element =
        { Element = element
          ApplicationSkillId = id
          ApplicationSkillIcdMs = 200f
          GaugeUnits = GaugeUnits.wrap gu
          Permanent = false } |> ElementalAura.wrap

    let createPermanentTrigger gu id element =
        element
        |> createGenericTrigger gu id
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