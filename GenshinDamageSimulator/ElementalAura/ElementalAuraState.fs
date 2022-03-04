namespace GenshinDamageSimulator

open System.Collections.Generic
open System.Linq

exception InvalidElementalAuraStateException of string * Element

type ElementalAuraState = ElementalAuraState of Map<Element, ElementalAura>

type ElementalAuraStateResult =
    { ElectroChargedTicks: int
      BurningTicks: int }

module ElementalAuraState =
    let wrap state =
        state |> ElementalAuraState

    let unwrap state =
        let (ElementalAuraState state') = state
        state'

    let empty = Map.empty |> wrap

    let add aura state =
        let el = ElementalAura.element aura
        state |> unwrap |> Map.add el aura |> wrap

    let remove el state =
        state |> unwrap |> Map.remove el |> wrap

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

    /// Calculates the aura result of a single tick of Electro-Charged.
    let private tickec aura =
        // https://library.keqingmains.com/combat-mechanics/elemental-effects/elemental-gauge-theory#electro-charged
        match aura with
        | ElectroAura a -> { a with Gauge = a.Gauge - 0.4f } |> ElementalAura.wrap
        | HydroAura a -> { a with Gauge = a.Gauge - 0.4f } |> ElementalAura.wrap
        | _ -> aura

    /// Calculates the aura result of Electro-Charged application over the provided time in seconds,
    /// returning the updated auras, reaction state, and number of ticks that occurred.
    let rec private elapseec electro hydro r s ticks =
        // https://library.keqingmains.com/evidence/combat-mechanics/elemental-effects/transformative-reactions#ec-ticks-only-consume-gauge-when-they-deal-damage
        // The gauges still decay normally, so we need to account for this at each tick in this function
        let duration a = a |> ElementalAura.gauge |> Gauge.duration
        let extraTick a = a |> duration |> fun d -> 1f > d && d >= 0.5f
        match r with
        | Some (ElectroCharged ec) ->
            let t, _ = ElectroCharged.unwrap ec
            if ElementalAura.isExpired electro || ElementalAura.isExpired hydro then
                electro, hydro, None, ticks
            elif s < 0f then // Elapsed time expired
                electro, hydro, r, ticks
            elif s < t then // Not enough time to trigger an EC tick
                (electro |> ElementalAura.decay s), (hydro |> ElementalAura.decay s), Some (ElectroCharged (ec |> ElectroCharged.cooldown (t - s))), ticks
            else // Handle decay and tick
                let s' = s - t
                let electro' = electro |> ElementalAura.decay t |> tickec
                let hydro' = hydro |> ElementalAura.decay t |> tickec
                // Handle the extra tick that occurs when the remaining
                // duration d of either aura is 1.0>d>=0.5
                if extraTick electro' || extraTick hydro' then
                    elapseec electro' hydro' (Some (ElectroCharged (ElectroCharged.reset ec))) s' (ticks + 2)
                else
                    elapseec electro' hydro' (Some (ElectroCharged (ElectroCharged.reset ec))) s' (ticks + 1)
        | _ -> electro, hydro, r, 0

    /// Calculates the aura result of Burning application over the provided time in seconds,
    /// returning the updated auras, reaction state, and number of ticks that occurred.
    let rec private elapseb pyro dendro r s ticks =
        let decayDendro d p s =
            let dg = d |> ElementalAura.gauge
            let dd = d |> ElementalAura.gauge |> Gauge.dr
            let pd = p |> ElementalAura.gauge |> Gauge.dr
            { ElementalAura.unwrap d with Gauge = dg - s / (dd + pd) } |> ElementalAura.wrap
        match r with
        | Some (Burning t) ->
            if ElementalAura.isExpired pyro || ElementalAura.isExpired dendro then
                pyro, dendro, None, ticks
            elif s < 0f then // Elapsed time expired
                pyro, dendro, r, ticks
            elif s < t then // Not enough time to trigger a Burning tick
                (pyro |> ElementalAura.decay s), (dendro |> ElementalAura.decay s), Some (Burning (t - s)), ticks
            else // Handle decay and tick
                let s' = s - t
                let pyro' = pyro |> ElementalAura.decay s'
                let pyroTrigger = // Pyro gets refreshed with a 2U AoE
                    { Element = Element.Pyro
                      ApplicationSkillId = 0u
                      ApplicationSkillIcdMs = 0f
                      ElementalMastery = 0f
                      Gauge = Gauge.ofUnits 2f
                      Permanent = false }
                let pyro'' = (ElementalAura.resolveGaugeAdd (ElementalAura.unwrap pyro') pyroTrigger |> Array.ofSeq).[0]
                let dendro' = decayDendro dendro pyro s'
                elapseb pyro'' dendro' (Some (Burning 0.25f)) s' (ticks + 1)
        | _ -> pyro, dendro, r, 0

    /// Calculates the aura result of gauge decay over the provided time in seconds.
    let private elapse1 aura s =
        let ad = aura |> ElementalAura.unwrap
        let ad' = { ad with Gauge = ad.Gauge |> Gauge.decay s }
        ad' |> ElementalAura.wrap

    let private addIfLiveMap a s =
        let el = ElementalAura.element a
        if not (a |> ElementalAura.isExpired) then Map.add el a s else s

    let private addIfLive a s =
        if not (a |> ElementalAura.isExpired) then add a s else s

    /// Elapses s0 with entries from s1, where s0 and s1 do not share keys.
    let rec private elapseFillWith s1 t s0 =
        if Map.isEmpty s1 then
            s0
        else
            let (k, v) = s1 |> Map.pick (fun k v -> Some (k, v))
            let s1' = Map.remove k s1
            if not (s0 |> Map.containsKey k) then
                let v' = elapse1 v t
                let s0' = addIfLiveMap v' s0
                elapseFillWith s1' t s0'
            else
                elapseFillWith s1' t s0

    /// Elapse the provided states, considering only elements involved in reactions.
    let rec private elapser s1 r1 s0 r0 s ec b =
        match r0 with
        | r :: r0' ->
            match r with
            | ElectroCharged _ ->
                let electro = s0 |> get Element.Electro
                let hydro = s0 |> get Element.Hydro
                let electro', hydro', r'o, ticks = elapseec electro hydro (Some r) s 0
                let s1' = s1 |> addIfLive electro' |> addIfLive hydro'
                let ec' = ec + ticks
                match r'o with
                | Some r' -> elapser s1' (r' :: r1) s0 r0' s ec' b
                | None -> elapser s1' r1 s0 r0' s ec' b
            | Burning _ ->
                let pyro = s0 |> get Element.Pyro
                let dendro = s0 |> get Element.Dendro
                let pyro', dendro', r'o, ticks = elapseb pyro dendro (Some r) s 0
                let s1' = s1 |> addIfLive pyro' |> addIfLive dendro'
                let b' = b + ticks
                match r'o with
                | Some r' -> elapser s1' (r' :: r1) s0 r0' s ec b'
                | None -> elapser s1' r1 s0 r0' s ec b'
            | _ -> elapser s1 r1 s0 r0 s ec b
        | _ -> s1, r1, ec, b

    /// Calculates the resulting aura state after elapsing the specified time in seconds, considering reactions.
    let elapse state reactions s =
        let state', reactions', ecticks, bticks = elapser empty [] state reactions s 0 0
        let state'' = state' |> unwrap |> elapseFillWith (state |> unwrap) s |> wrap
        let result = { ElectroChargedTicks = ecticks; BurningTicks = bticks }
        state'', reactions', result

    let private interactEmpty state trigger =
        let element = ElementalAura.element trigger
        match element with
        | Element.Geo | Element.Anemo -> state, Seq.empty
        | _ ->
            let auraData = ElementalAura.unwrap trigger
            let taxedGu = auraData.Gauge |> Gauge.tax
            let newState = state |> unwrap |> Map.add element (ElementalAura.wrap { auraData with Gauge = taxedGu })
            wrap newState, Seq.empty

    let rec private interactne s1 r s0 trigger =
        if s0 |> isEmpty then
            s1, r |> Seq.ofList
        else
            let (el, a) = s0 |> unwrap |> Map.pick (fun k v -> Some (k, v))
            let aa, r'o = ElementalAura.interact a trigger
            let s0' = s0 |> remove el
            let s1' = (s1, aa) ||> Seq.fold (fun s a -> add a s)
            match r'o with
            | Some r' -> interactne s1' (r' :: r) s0' trigger
            | _ -> interactne s1' r s0' trigger

    let private interactNotEmpty =
        interactne empty []

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