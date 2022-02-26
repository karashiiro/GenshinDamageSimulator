namespace GenshinDamageSimulator

open System.Collections.Generic
open System.Linq

exception InvalidElementalAuraStateException of string * Element

type ElementalAuraState = ElementalAuraState of Map<Element, ElementalAura>

module ElementalAuraState =
    let wrap state =
        state |> ElementalAuraState

    let unwrap state =
        let (ElementalAuraState state') = state
        state'

    let empty = Map.empty |> wrap

    let contains element state =
        state |> unwrap |> Map.containsKey element

    let get element state =
        state |> unwrap |> Map.find element

    let tryGet element state =
        state |> unwrap |> Map.tryFind element

    let isSingle state =
        state |> unwrap |> Map.count |> (=) 1

    let isEmpty state =
        state |> unwrap |> Map.isEmpty

    let interactEmpty state trigger =
        let element = ElementalAura.element trigger
        match element with
        | Element.Geo | Element.Anemo -> state, Seq.empty
        | _ ->
            let auraData = ElementalAura.unwrap trigger
            let taxedGu = auraData.GaugeUnits |> GaugeUnits.unwrap |> (*) 0.8f |> GaugeUnits.wrap
            let newState = state |> unwrap |> Map.add element (ElementalAura.wrap { auraData with GaugeUnits = taxedGu })
            wrap newState, Seq.empty

    let interactNotEmpty state trigger =
        state
        |> unwrap
        |> Map.values
        |> Seq.map (fun aura -> ElementalAura.interact aura trigger)
        |> Seq.map (fun (auras, reaction) -> auras, reaction |> Option.toArray |> Seq.ofArray)
        |> (Seq.fold (fun (x, y) (auras, reactions) ->
            let allAuras = Seq.append x auras
            let allReactions = Seq.append y reactions
            allAuras, allReactions) (Seq.empty, Seq.empty))
        ||> fun auras reactions -> Seq.map (fun aura -> ElementalAura.element aura, aura) auras, reactions
        ||> fun auras reactions -> auras |> Map.ofSeq |> wrap, reactions

    let interact state =
        if state |> isEmpty then interactEmpty state else interactNotEmpty state

// This is the C# interface for the aura state.
type ElementalAuraState with
    /// Creates a new elemental aura state object.
    static member Create() = ElementalAuraState.empty

    /// Creates a new elemental aura state object from the provided dictionary.
    static member FromDictionary (dict: IDictionary<Element, ElementalAuraData>) =
        if isNull (box dict) then nullArg "dict"
        for kvp in dict.AsEnumerable() do
            if kvp.Key <> kvp.Value.Element then
                raise (InvalidElementalAuraStateException("Input key does not match aura data value.", kvp.Key))
        dict.AsEnumerable().Select(fun kvp -> kvp.Key, kvp.Value |> ElementalAura.wrap)
        |> Map.ofSeq
        |> ElementalAuraState