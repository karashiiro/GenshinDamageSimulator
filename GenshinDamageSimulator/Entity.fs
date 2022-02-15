namespace GenshinDamageSimulator

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

    let getBNpcBaseStat bNpc stat =
        match stat with
        | BaseStat.Hp -> bNpc.BaseHp
        | BaseStat.Attack -> bNpc.BaseAttack
        | BaseStat.Defense -> bNpc.BaseDefense

    let getBNpcBaseResStat bNpc resStat = 
        match resStat with
        | DamageType.Physical -> bNpc.BasePhysicalRes
        | DamageType.Pyro -> bNpc.BasePyroRes
        | DamageType.Hydro -> bNpc.BaseHydroRes
        | DamageType.Electro -> bNpc.BaseElectroRes
        | DamageType.Cryo -> bNpc.BaseCryoRes
        | DamageType.Anemo -> bNpc.BaseAnemoRes
        | DamageType.Geo -> bNpc.BaseGeoRes
        | DamageType.Dendro -> bNpc.BaseDendroRes

    let getBNpcDamageBonusPercent bNpc damageType =
        match damageType with
        | DamageType.Physical -> getBNpcStatPercent bNpc PercStat.Physical
        | DamageType.Pyro -> getBNpcStatPercent bNpc PercStat.Pyro
        | DamageType.Hydro -> getBNpcStatPercent bNpc PercStat.Hydro
        | DamageType.Electro -> getBNpcStatPercent bNpc PercStat.Electro
        | DamageType.Cryo -> getBNpcStatPercent bNpc PercStat.Cryo
        | DamageType.Anemo -> getBNpcStatPercent bNpc PercStat.Anemo
        | DamageType.Geo -> getBNpcStatPercent bNpc PercStat.Geo
        | DamageType.Dendro -> getBNpcStatPercent bNpc PercStat.Dendro

    let getBNpcDamageResPercent bNpc damageType =
        match damageType with
        | DamageType.Physical -> getBNpcStatPercent bNpc PercStat.PhysicalRes
        | DamageType.Pyro -> getBNpcStatPercent bNpc PercStat.PyroRes
        | DamageType.Hydro -> getBNpcStatPercent bNpc PercStat.HydroRes
        | DamageType.Electro -> getBNpcStatPercent bNpc PercStat.ElectroRes
        | DamageType.Cryo -> getBNpcStatPercent bNpc PercStat.CryoRes
        | DamageType.Anemo -> getBNpcStatPercent bNpc PercStat.AnemoRes
        | DamageType.Geo -> getBNpcStatPercent bNpc PercStat.GeoRes
        | DamageType.Dendro -> getBNpcStatPercent bNpc PercStat.DendroRes