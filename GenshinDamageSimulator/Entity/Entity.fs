﻿namespace GenshinDamageSimulator

type Artifact =
    { MainStat: StatModifier
      StatLines: StatModifier array }

type Weapon =
    { Attack: float32
      MainStat: StatModifier option }

type BasicEntityData =
    { BaseHp: float32
      BaseAttack: float32
      BaseDefense: float32
      BasePhysicalRes: float32
      BasePyroRes: float32
      BaseHydroRes: float32
      BaseElectroRes: float32
      BaseCryoRes: float32
      BaseAnemoRes: float32
      BaseGeoRes: float32
      BaseDendroRes: float32
      Level: uint32 } // TODO: This isn't necessarily fixed, but we'll consider it so for now

type CharacterEntityData =
    { MainStat: StatModifier
      CriticalHit: StatModifier
      CriticalDamage: StatModifier
      EnergyRecharge: StatModifier
      Element: Element option
      Weapon: Weapon
      Artifacts: Artifact array }

type CharacterEntity = BasicEntityData * CharacterEntityData

type Entity =
    | EnemyEntity of BasicEntityData
    | CharacterEntity of CharacterEntity

type EntityState = // TODO: Status effects
    { Id: EntityId
      Hp: float32
      ShieldHp: float32
      ElementalAuras: ElementalAuraState }

module Entity =
    let getTotalFlat stat statLines =
        statLines
        |> List.map (fun x -> match x with
                              | FlatStatModifier f
                                  -> match f.Type with
                                     | s when s = stat -> f.Value
                                     | _ -> 0f
                              | _ -> 0f)
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

    let getStatLines entity =
        match entity with
        | CharacterEntity (_, cd)
            ->
                let statLines =
                    match cd.Weapon.MainStat with
                    | Some weaponMainStat -> weaponMainStat :: (cd.Artifacts |> Seq.map (fun x -> x.MainStat :: List.ofArray x.StatLines) |> Seq.concat |> List.ofSeq)
                    | None -> cd.Artifacts |> Seq.map (fun x -> x.MainStat :: List.ofArray x.StatLines) |> Seq.concat |> List.ofSeq
                cd.MainStat :: cd.CriticalHit :: cd.CriticalDamage :: cd.EnergyRecharge :: statLines
        | EnemyEntity _ -> []

    let getStatFlat stat entity =
        entity |> getStatLines |> getTotalFlat stat

    let getStatPercent stat entity =
        entity |> getStatLines |> getTotalPercent stat

    let getBaseStat stat entity =
        let basicData = match entity with
                        | CharacterEntity (cd, _) -> cd
                        | EnemyEntity ed -> ed
        match stat with
        | BaseStat.Hp -> basicData.BaseHp
        | BaseStat.Attack -> basicData.BaseAttack
        | BaseStat.Defense -> basicData.BaseDefense

    let getBaseResStat resStat entity = 
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

    let getDamageBonusPercent damageType =
        match damageType with
        | DamageType.Physical -> getStatPercent PercStat.PhysicalBonus
        | DamageType.Pyro -> getStatPercent PercStat.PyroBonus
        | DamageType.Hydro -> getStatPercent PercStat.HydroBonus
        | DamageType.Electro -> getStatPercent PercStat.ElectroBonus
        | DamageType.Cryo -> getStatPercent PercStat.CryoBonus
        | DamageType.Anemo -> getStatPercent PercStat.AnemoBonus
        | DamageType.Geo -> getStatPercent PercStat.GeoBonus
        | DamageType.Dendro -> getStatPercent PercStat.DendroBonus

    let getDamageResPercent damageType =
        match damageType with
        | DamageType.Physical -> getStatPercent PercStat.PhysicalRes
        | DamageType.Pyro -> getStatPercent PercStat.PyroRes
        | DamageType.Hydro -> getStatPercent PercStat.HydroRes
        | DamageType.Electro -> getStatPercent PercStat.ElectroRes
        | DamageType.Cryo -> getStatPercent PercStat.CryoRes
        | DamageType.Anemo -> getStatPercent PercStat.AnemoRes
        | DamageType.Geo -> getStatPercent PercStat.GeoRes
        | DamageType.Dendro -> getStatPercent PercStat.DendroRes

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

/// A basic entity data mapping class for use in C#.
type BasicEntityParams() =
    member val BaseHp = 0f with get, set
    member val BaseAttack = 0f with get, set
    member val BaseDefense = 0f with get, set
    member val BasePhysicalRes = 0f with get, set
    member val BasePyroRes = 0f with get, set
    member val BaseHydroRes = 0f with get, set
    member val BaseCryoRes = 0f with get, set
    member val BaseElectroRes = 0f with get, set
    member val BaseAnemoRes = 0f with get, set
    member val BaseGeoRes = 0f with get, set
    member val BaseDendroRes = 0f with get, set
    member val Level = 0u with get, set

    member this.ToBasicEntityData () =
        { BaseHp = this.BaseHp
          BaseAttack = this.BaseAttack
          BaseDefense = this.BaseDefense
          BasePhysicalRes = this.BasePhysicalRes
          BasePyroRes = this.BasePyroRes
          BaseHydroRes = this.BaseHydroRes
          BaseCryoRes = this.BaseCryoRes
          BaseElectroRes = this.BaseElectroRes
          BaseAnemoRes = this.BaseAnemoRes
          BaseGeoRes = this.BaseGeoRes
          BaseDendroRes = this.BaseDendroRes
          Level = this.Level }

/// A character entity data mapping class for use in C#.
type CharacterEntityParams() =
    member val MainStat = FlatStatModifier { Type = FlatStat.Attack; Value = 0f } with get, set
    member val CriticalHit = PercStatModifier { Type = PercStat.CriticalHit; Value = 0f } with get, set
    member val CriticalDamage = PercStatModifier { Type = PercStat.CriticalDamage; Value = 0f } with get, set
    member val EnergyRecharge = PercStatModifier { Type = PercStat.EnergyRecharge; Value = 0f } with get, set
    member val Element = None with get, set
    member val Weapon = { Attack = 0f; MainStat = None } with get, set
    member val Artifacts = Array.empty with get, set

    member this.ToCharacterEntityData () =
        if isNull (box this.MainStat) then nullArg "MainStat"
        if isNull (box this.CriticalHit) then nullArg "CriticalHit"
        if isNull (box this.CriticalDamage) then nullArg "CriticalDamage"
        if isNull (box this.EnergyRecharge) then nullArg "EnergyRecharge"
        if isNull (box this.Weapon) then nullArg "Weapon"
        if isNull (box this.Artifacts) then nullArg "Artifacts"
        { MainStat = this.MainStat
          CriticalHit = this.CriticalHit
          CriticalDamage = this.CriticalDamage
          EnergyRecharge = this.EnergyRecharge
          Element = this.Element
          Weapon = this.Weapon
          Artifacts = this.Artifacts }

// This is the C# interface for entities.
type Entity with
    /// Creates a new character entity from the provided data. This method should be preferred
    /// over NewCharacterEntity.
    static member CreateCharacter (basicData: BasicEntityParams) (characterData: CharacterEntityParams) =
        if isNull (box basicData) then nullArg "basicData"
        if isNull (box characterData) then nullArg "characterData"
        (basicData.ToBasicEntityData(), characterData.ToCharacterEntityData()) |> CharacterEntity

    /// Creates a new enemy entity from the provided data. This method should be preferred
    /// over NewEnemyEntity.
    static member CreateEnemy (basicData: BasicEntityParams) =
        if isNull (box basicData) then nullArg "basicData"
        basicData.ToBasicEntityData() |> EnemyEntity

    /// Returns the max HP of this entity.
    member this.GetMaxHp () =
        let statLines = Entity.getStatLines this
        ((Entity.getBaseStat BaseStat.Hp this) + (Entity.getTotalFlat FlatStat.Hp statLines)) * (1f + Entity.getTotalPercent PercStat.Hp statLines)