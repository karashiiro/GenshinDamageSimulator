namespace GenshinDamageSimulator

type ElementalResonance =
    | FerventFlames
    | SoothingWater
    | HighVoltage
    | ShatteringIce
    | ImpetuousWinds
    | EnduringRock
    | ProtectiveCanopy

type Party = Map<EntityId, CharacterEntity>

module Party =
    let private hasProtectiveCanopy (map: Map<Element, int>) =
        map.Count >= 4

    let private hasTwo element map =
        map
        |> Map.tryFind element
        |> fun res
            -> match res with
               | Some x -> x >= 2
               | None -> false

    let private hasFerventFlames =
        hasTwo Element.Pyro

    let private hasSoothingWater =
        hasTwo Element.Hydro

    let private hasHighVoltage =
        hasTwo Element.Electro

    let private hasShatteringIce =
        hasTwo Element.Cryo

    let private hasImpetuousWinds =
        hasTwo Element.Anemo

    let private hasEnduringRock =
        hasTwo Element.Geo

    let getResonances (party: Party) =
        party.Values
        |> Seq.filter (fun (_, c) -> Option.isSome c.Element)
        |> Seq.countBy (fun (_, c) -> Option.get c.Element)
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
        if hasPyroResonance then 0.25f else 0f