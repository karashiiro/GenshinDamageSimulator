﻿namespace GenshinDamageSimulator

module Entity =
    let getTotalFlat stat statLines =
        statLines
        |> List.map (fun x -> match x with
                              | FlatStatModifier f
                                  -> match f.Type with
                                     | s when s = stat -> f.Value
                                     | _ -> 0u
                              | _ -> 0u)
        |> List.sum

    let getTotalPercent stat statLines =
        statLines
        |> List.map (fun x -> match x with
                              | PercStatModifier p
                                  -> match p.Type with
                                     | s when s = stat -> p.Value
                                     | _ -> 0f
                              | _ -> 0f)
        |> List.sum

    let getBNpcStatLines bNpc =
        bNpc.MainStat :: bNpc.Weapon.MainStat :: (bNpc.Artifacts |> Seq.map (fun x -> x.MainStat :: List.ofArray x.StatLines) |> Seq.concat |> List.ofSeq)

    let getBNpcStatFlat stat bNpc =
        bNpc
        |> getBNpcStatLines
        |> getTotalFlat stat

    let getBNpcStatPercent stat bNpc =
        bNpc
        |> getBNpcStatLines
        |> getTotalPercent stat

    let getBNpcBaseStat stat bNpc =
        match stat with
        | BaseStat.Hp -> bNpc.BaseHp
        | BaseStat.Attack -> bNpc.BaseAttack
        | BaseStat.Defense -> bNpc.BaseDefense

    let getBNpcBaseResStat resStat bNpc = 
        match resStat with
        | DamageType.Physical -> bNpc.BasePhysicalRes
        | DamageType.Pyro -> bNpc.BasePyroRes
        | DamageType.Hydro -> bNpc.BaseHydroRes
        | DamageType.Electro -> bNpc.BaseElectroRes
        | DamageType.Cryo -> bNpc.BaseCryoRes
        | DamageType.Anemo -> bNpc.BaseAnemoRes
        | DamageType.Geo -> bNpc.BaseGeoRes
        | DamageType.Dendro -> bNpc.BaseDendroRes

    let getBNpcDamageBonusPercent damageType =
        match damageType with
        | DamageType.Physical -> getBNpcStatPercent PercStat.Physical
        | DamageType.Pyro -> getBNpcStatPercent PercStat.Pyro
        | DamageType.Hydro -> getBNpcStatPercent PercStat.Hydro
        | DamageType.Electro -> getBNpcStatPercent PercStat.Electro
        | DamageType.Cryo -> getBNpcStatPercent PercStat.Cryo
        | DamageType.Anemo -> getBNpcStatPercent PercStat.Anemo
        | DamageType.Geo -> getBNpcStatPercent PercStat.Geo
        | DamageType.Dendro -> getBNpcStatPercent PercStat.Dendro

    let getBNpcDamageResPercent damageType =
        match damageType with
        | DamageType.Physical -> getBNpcStatPercent PercStat.PhysicalRes
        | DamageType.Pyro -> getBNpcStatPercent PercStat.PyroRes
        | DamageType.Hydro -> getBNpcStatPercent PercStat.HydroRes
        | DamageType.Electro -> getBNpcStatPercent PercStat.ElectroRes
        | DamageType.Cryo -> getBNpcStatPercent PercStat.CryoRes
        | DamageType.Anemo -> getBNpcStatPercent PercStat.AnemoRes
        | DamageType.Geo -> getBNpcStatPercent PercStat.GeoRes
        | DamageType.Dendro -> getBNpcStatPercent PercStat.DendroRes