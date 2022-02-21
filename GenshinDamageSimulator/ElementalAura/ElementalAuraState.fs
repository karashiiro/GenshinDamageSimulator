namespace GenshinDamageSimulator

open System.Collections.Generic
open System.Linq

type ElementalAuraState = ElementalAuraState of Map<Element, ElementalAura>

module ElementalAuraState =
    let unwrap state =
        let (ElementalAuraState state') = state
        state'

    let interact state trigger =
        state
        |> Map.values
        |> Seq.map (fun aura -> ElementalAura.interact aura trigger)
        |> Seq.map (fun (auras, reaction) -> auras, reaction |> Option.toArray |> Seq.ofArray)
        |> (Seq.fold (fun (x, y) (auras, reactions) ->
            let allAuras = Seq.append x auras
            let allReactions = Seq.append y reactions
            allAuras, allReactions) (Seq.empty, Seq.empty))
        ||> fun auras reactions -> Seq.map (fun aura -> ElementalAura.getAuraElement aura, aura) auras, reactions
        ||> fun auras reactions -> Map.ofSeq auras, reactions

// This is the C# interface for the aura state.
type ElementalAuraState with
    /// Creates a new elemental aura state object.
    static member Create() = Map.empty |> ElementalAuraState

    /// Creates a new elemental aura state object from the provided dictionary.
    static member FromDictionary (dict: IDictionary<Element, ElementalAura>) =
        if isNull (box dict) then nullArg "dict"
        dict.AsEnumerable().Select(fun kvp -> kvp.Key, kvp.Value)
        |> Map.ofSeq
        |> ElementalAuraState