namespace Scryglass

open Microsoft.FSharp.Data.UnitSystems.SI.UnitNames
open Scryglass

[<AutoOpen>]
module EntityTypes =
    type Artifact =
        { MainStat: StatModifier
          SubStats: StatModifier list }

    type Weapon =
        { Attack: FlatAttack
          MainStat: StatModifier option }

    type BasicEntityData =
        { BaseHp: FlatHp
          BaseAttack: FlatAttack
          BaseDefense: FlatDefense
          BaseElementalMastery: FlatElementalMastery
          BasePhysicalRes: PercentPhysicalRes
          BasePyroRes: PercentPyroRes
          BaseHydroRes: PercentHydroRes
          BaseElectroRes: PercentElectroRes
          BaseCryoRes: PercentCryoRes
          BaseAnemoRes: PercentAnemoRes
          BaseGeoRes: PercentGeoRes
          BaseDendroRes: PercentDendroRes
          Level: Level }

    type CharacterEntityData =
        { MainStat: StatModifier
          BaseCriticalRate: PercentCriticalRate
          BaseCriticalDamage: PercentCriticalDamage
          BaseEnergyRecharge: PercentEnergyRecharge
          Weapon: Weapon
          Artifacts: Artifact list }

    type InternalCooldownGroupId = uint
    type InternalCooldown = InternalCooldownGroupId * float<second>

    type Shield =
        { CurrentHp: StatValue
          Strength: StatValue }

    type EntityState = // buffs and debuffs?
        { CurrentHp: StatValue
          InternalCooldowns: InternalCooldown list }

    type CharacterEntity = EntityState * BasicEntityData * CharacterEntityData
    
    type Entity =
        | EnemyEntity of EntityState * BasicEntityData
        | CharacterEntity of CharacterEntity

    [<RequireQualifiedAccess>]
    type TalentScalingStat = Hp | Attack | Defense
    type Talent =
        { ScalingStat: TalentScalingStat
          ScalingStatMultiplier: float
          Aura: Aura option }

module CharacterEntity =
    let private percentHp stat =
        match stat with
        | PercentHp hp -> hp
        | _ -> 0.0

    let private flatHp stat =
        match stat with
        | FlatHp hp -> hp
        | _ -> 0.0

    let private percentAttack stat =
        match stat with
        | PercentAttack atk -> atk
        | _ -> 0.0

    let private flatAttack stat =
        match stat with
        | FlatAttack atk -> atk
        | _ -> 0.0

    let private percentDefense stat =
        match stat with
        | PercentDefense def -> def
        | _ -> 0.0

    let private flatDefense stat =
        match stat with
        | FlatDefense def -> def
        | _ -> 0.0

    let private percentCriticalRate stat =
        match stat with
        | PercentCriticalRate chit -> chit
        | _ -> 0.0

    let private percentCriticalDamage stat =
        match stat with
        | PercentCriticalDamage cdmg -> cdmg
        | _ -> 0.0

    let private artifactStats f (artifact: Artifact) =
        ((0.0, artifact.SubStats) ||> List.fold (fun s v -> s + f v)) + f artifact.MainStat

    let private totalStats f (character: CharacterEntity) =
        let _, _, characterData = character
        let mainStat = f characterData.MainStat
        let weaponStat =
            match characterData.Weapon.MainStat with
            | Some s -> f s
            | None -> 0.0
        let artifactsStat = (0.0, characterData.Artifacts) ||> List.fold (fun s a -> s + artifactStats f a)
        mainStat + weaponStat + artifactsStat

    let totalHp ((state, data, characterData): CharacterEntity) =
        let character = (state, data, characterData)
        data.BaseHp * (1.0 + totalStats percentHp character) + totalStats flatHp character

    let totalAttack ((state, data, characterData): CharacterEntity) =
        let character = (state, data, characterData)
        (data.BaseAttack + characterData.Weapon.Attack) * (1.0 + totalStats percentAttack character) + totalStats flatAttack character

    let totalDefense ((state, data, characterData): CharacterEntity) =
        let character = (state, data, characterData)
        data.BaseDefense * (1.0 + totalStats percentDefense character) + totalStats flatDefense character

    let totalCriticalRate ((state, data, characterData): CharacterEntity) =
        let character = (state, data, characterData)
        characterData.BaseCriticalRate + totalStats percentCriticalRate character

    let totalCriticalDamage ((state, data, characterData): CharacterEntity) =
        let character = (state, data, characterData)
        characterData.BaseCriticalRate + totalStats percentCriticalDamage character

module Entity =
    let totalHp entity =
        match entity with
        | EnemyEntity (_, data) -> data.BaseHp
        | CharacterEntity character -> CharacterEntity.totalHp character

    let totalAttack entity =
        match entity with
        | EnemyEntity (_, data) -> data.BaseAttack
        | CharacterEntity character -> CharacterEntity.totalAttack character

    let totalDefense entity =
        match entity with
        | EnemyEntity (_, data) -> data.BaseDefense
        | CharacterEntity character -> CharacterEntity.totalDefense character

    let totalCriticalRate entity =
        match entity with
        | EnemyEntity _ -> 0.0
        | CharacterEntity character -> CharacterEntity.totalCriticalRate character

    let totalCriticalDamage entity =
        match entity with
        | EnemyEntity _ -> 0.0
        | CharacterEntity character -> CharacterEntity.totalCriticalDamage character