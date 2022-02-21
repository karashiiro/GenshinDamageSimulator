namespace GenshinDamageSimulator

type ElementalAuraState = Map<Element, ElementalAura>

module ElementalAuraState =
    let interact state trigger =
        state
        |> Map.values
        |> Seq.map (fun aura -> ElementalAura.interact1 aura trigger)
        |> Seq.map (fun (auras, reaction) -> auras, reaction |> Option.toArray |> Seq.ofArray)
        |> (Seq.fold (fun (x, y) (auras, reactions) ->
            let allAuras = Seq.append x auras
            let allReactions = Seq.append y reactions
            allAuras, allReactions) (Seq.empty, Seq.empty))
        ||> fun auras reactions -> Seq.map (fun aura -> ElementalAura.getAuraElement aura, aura) auras, reactions
        ||> fun auras reactions -> Map.ofSeq auras, reactions