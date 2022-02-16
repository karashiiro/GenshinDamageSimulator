namespace GenshinDamageSimulator

module Resonance =
    let hasProtectiveCanopy (map: Map<Element, int>) =
        map.Count >= 4

    let hasTwo element map =
        map
        |> Map.tryFind element
        |> fun res
            -> match res with
               | Some x -> x >= 2
               | None -> false

    let hasFerventFlames =
        hasTwo Element.Pyro

    let hasSoothingWater =
        hasTwo Element.Hydro

    let hasHighVoltage =
        hasTwo Element.Electro

    let hasShatteringIce =
        hasTwo Element.Cryo

    let hasImpetuousWinds =
        hasTwo Element.Anemo

    let hasEnduringRock =
        hasTwo Element.Geo

    let getResonances party =
        party
        |> Seq.countBy (fun x -> x.Element)
        |> Map.ofSeq
        |> fun map -> ([], map)
        ||> fun resonances map -> if hasProtectiveCanopy map then (ProtectiveCanopy :: resonances, map) else (resonances, map)
        ||> fun resonances map -> if hasFerventFlames map then (FerventFlames :: resonances, map) else (resonances, map)
        ||> fun resonances map -> if hasSoothingWater map then (SoothingWater :: resonances, map) else (resonances, map)
        ||> fun resonances map -> if hasHighVoltage map then (HighVoltage :: resonances, map) else (resonances, map)
        ||> fun resonances map -> if hasShatteringIce map then (ShatteringIce :: resonances, map) else (resonances, map)
        ||> fun resonances map -> if hasImpetuousWinds map then (ImpetuousWinds :: resonances, map) else (resonances, map)
        ||> fun resonances map -> if hasEnduringRock map then (EnduringRock :: resonances, map) else (resonances, map)
        ||> fun resonances _ -> resonances
        |> Set.ofList

    let calcResonanceAttackPercent party =
        let hasPyroResonance =
            party
            |> getResonances
            |> Set.contains FerventFlames
        if hasPyroResonance then 0.25f else 0.0f