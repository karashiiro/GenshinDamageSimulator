namespace GenshinDamageSimulator

open StatTypes
open EntityTypes

module Entity =
    let getTotalFlat stat statLines =
        statLines
        |> List.map (fun x -> match x with
                                | FlatStatModifier f -> match f.Type with
                                                        | s when s = stat -> f.Value
                                                        | _ -> 0u
                                | _ -> 0u)
        |> List.sum

    let getTotalPercent stat statLines =
        statLines
        |> List.map (fun x -> match x with
                                | PercStatModifier p -> match p.Type with
                                                        | s when s = stat -> p.Value
                                                        | _ -> 0f
                                | _ -> 0f)
        |> List.sum

    let getBNpcStatLines bNpc =
        bNpc.MainStat :: bNpc.Weapon.MainStat :: (bNpc.Artifacts |> Seq.map (fun x -> x.MainStat :: List.ofArray x.StatLines) |> Seq.concat |> List.ofSeq)

    let getBNpcStatFlat bNpc stat =
        bNpc
        |> getBNpcStatLines
        |> getTotalFlat stat

    let getBNpcStatPercent bNpc stat =
        bNpc
        |> getBNpcStatLines
        |> getTotalPercent stat