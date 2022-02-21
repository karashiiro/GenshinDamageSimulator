namespace GenshinDamageSimulator

module EntityLogic =
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

    let getBNpcStatFlat stat entity =
        match entity with
        | CharacterEntity (_, cd) -> cd |> getBNpcStatLines |> getTotalFlat stat
        | EnemyEntity _ -> 0u

    let getBNpcStatPercent stat entity =
        match entity with
        | CharacterEntity (_, cd) -> cd |> getBNpcStatLines |> getTotalPercent stat
        | EnemyEntity _ -> 0f

    let getBNpcBaseStat stat entity =
        let basicData = match entity with
                        | CharacterEntity (cd, _) -> cd
                        | EnemyEntity ed -> ed
        match stat with
        | BaseStat.Hp -> basicData.BaseHp
        | BaseStat.Attack -> basicData.BaseAttack
        | BaseStat.Defense -> basicData.BaseDefense

    let getBNpcBaseResStat resStat entity = 
        let basicData = match entity with
                        | CharacterEntity (cd, _) -> cd
                        | EnemyEntity ed -> ed
        match resStat with
        | DamageType.Physical -> basicData.BasePhysicalRes
        | DamageType.Pyro -> basicData.BasePyroRes
        | DamageType.Hydro -> basicData.BaseHydroRes
        | DamageType.Electro -> basicData.BaseElectroRes
        | DamageType.Cryo -> basicData.BaseCryoRes
        | DamageType.Anemo -> basicData.BaseAnemoRes
        | DamageType.Geo -> basicData.BaseGeoRes
        | DamageType.Dendro -> basicData.BaseDendroRes

    let getBNpcDamageBonusPercent damageType =
        match damageType with
        | DamageType.Physical -> getBNpcStatPercent PercStat.PhysicalBonus
        | DamageType.Pyro -> getBNpcStatPercent PercStat.PyroBonus
        | DamageType.Hydro -> getBNpcStatPercent PercStat.HydroBonus
        | DamageType.Electro -> getBNpcStatPercent PercStat.ElectroBonus
        | DamageType.Cryo -> getBNpcStatPercent PercStat.CryoBonus
        | DamageType.Anemo -> getBNpcStatPercent PercStat.AnemoBonus
        | DamageType.Geo -> getBNpcStatPercent PercStat.GeoBonus
        | DamageType.Dendro -> getBNpcStatPercent PercStat.DendroBonus

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

    let getElementForDamageType damageType =
        match damageType with
        | DamageType.Pyro -> Some(Element.Pyro)
        | DamageType.Hydro -> Some(Element.Hydro)
        | DamageType.Electro -> Some(Element.Electro)
        | DamageType.Cryo -> Some(Element.Cryo)
        | DamageType.Anemo -> Some(Element.Anemo)
        | DamageType.Geo -> Some(Element.Geo)
        | DamageType.Dendro -> Some(Element.Dendro)
        | _ -> None