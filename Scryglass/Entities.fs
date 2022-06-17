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
          InternalCooldowns: InternalCooldown list
          Effects: StatModifier list }

    type EnemyEntity = EntityState * BasicEntityData
    type CharacterEntity = EntityState * BasicEntityData * CharacterEntityData
    
    type Entity =
        | EnemyEntity of EnemyEntity
        | CharacterEntity of CharacterEntity

    [<RequireQualifiedAccess>]
    type TalentScalingStat = Hp | Attack | Defense
    type Talent =
        { ScalingStat: TalentScalingStat
          ScalingStatMultiplier: float
          Aura: Aura option }

module EnemyEntity =
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

module CharacterEntity =
    let private percentHp stat =
        match stat with
        | PercentHp s -> s
        | _ -> 0.0

    let private flatHp stat =
        match stat with
        | FlatHp s -> s
        | _ -> 0.0

    let private percentAttack stat =
        match stat with
        | PercentAttack s -> s
        | _ -> 0.0

    let private flatAttack stat =
        match stat with
        | FlatAttack s -> s
        | _ -> 0.0

    let private percentDefense stat =
        match stat with
        | PercentDefense s -> s
        | _ -> 0.0

    let private flatDefense stat =
        match stat with
        | FlatDefense s -> s
        | _ -> 0.0

    let private flatDamageBonus stat =
        match stat with
        | FlatDamageBonus s -> s
        | _ -> 0.0

    let private percentCriticalRate stat =
        match stat with
        | PercentCriticalRate s -> s
        | _ -> 0.0

    let private percentCriticalDamage stat =
        match stat with
        | PercentCriticalDamage s -> s
        | _ -> 0.0

    let private percentPhysicalRes stat =
        match stat with
        | PercentPhysicalRes s -> s
        | _ -> 0.0

    let private percentPyroRes stat =
        match stat with
        | PercentPyroRes s -> s
        | _ -> 0.0

    let private percentHydroRes stat =
        match stat with
        | PercentHydroRes s -> s
        | _ -> 0.0

    let private percentElectroRes stat =
        match stat with
        | PercentElectroRes s -> s
        | _ -> 0.0

    let private percentCryoRes stat =
        match stat with
        | PercentCryoRes s -> s
        | _ -> 0.0

    let private percentAnemoRes stat =
        match stat with
        | PercentAnemoRes s -> s
        | _ -> 0.0

    let private percentGeoRes stat =
        match stat with
        | PercentGeoRes s -> s
        | _ -> 0.0

    let private percentDendroRes stat =
        match stat with
        | PercentDendroRes s -> s
        | _ -> 0.0

    let private percentPhysicalResIgnore stat =
        match stat with
        | PercentPhysicalResIgnore s -> s
        | _ -> 0.0

    let private percentPyroResIgnore stat =
        match stat with
        | PercentPyroResIgnore s -> s
        | _ -> 0.0

    let private percentHydroResIgnore stat =
        match stat with
        | PercentHydroResIgnore s -> s
        | _ -> 0.0

    let private percentElectroResIgnore stat =
        match stat with
        | PercentElectroResIgnore s -> s
        | _ -> 0.0

    let private percentCryoResIgnore stat =
        match stat with
        | PercentCryoResIgnore s -> s
        | _ -> 0.0

    let private percentAnemoResIgnore stat =
        match stat with
        | PercentAnemoResIgnore s -> s
        | _ -> 0.0

    let private percentGeoResIgnore stat =
        match stat with
        | PercentGeoResIgnore s -> s
        | _ -> 0.0

    let private percentDendroResIgnore stat =
        match stat with
        | PercentDendroResIgnore s -> s
        | _ -> 0.0

    let private artifactStats f (artifact: Artifact) =
        ((0.0, artifact.SubStats) ||> List.fold (fun s v -> s + f v)) + f artifact.MainStat

    let private effectStats f (effects: StatModifier list) =
        (0.0, effects) ||> List.fold (fun s v -> s + f v)
    
    let private totalStats f (character: CharacterEntity) =
        let state, _, characterData = character
        let mainStat = f characterData.MainStat
        let weaponStat =
            match characterData.Weapon.MainStat with
            | Some s -> f s
            | None -> 0.0
        let artifactsStat = (0.0, characterData.Artifacts) ||> List.fold (fun s a -> s + artifactStats f a)
        let effectsStat = effectStats f state.Effects
        mainStat + weaponStat + artifactsStat + effectsStat

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

    let totalPhysicalRes ((state, data, characterData): CharacterEntity) =
        let character = (state, data, characterData)
        data.BasePhysicalRes + totalStats percentPhysicalRes character

    let totalPyroRes ((state, data, characterData): CharacterEntity) =
        let character = (state, data, characterData)
        data.BasePyroRes + totalStats percentPyroRes character

    let totalHydroRes ((state, data, characterData): CharacterEntity) =
        let character = (state, data, characterData)
        data.BaseHydroRes + totalStats percentHydroRes character

    let totalElectroRes ((state, data, characterData): CharacterEntity) =
        let character = (state, data, characterData)
        data.BaseElectroRes + totalStats percentElectroRes character

    let totalCryoRes ((state, data, characterData): CharacterEntity) =
        let character = (state, data, characterData)
        data.BaseCryoRes + totalStats percentCryoRes character

    let totalAnemoRes ((state, data, characterData): CharacterEntity) =
        let character = (state, data, characterData)
        data.BaseAnemoRes + totalStats percentAnemoRes character

    let totalGeoRes ((state, data, characterData): CharacterEntity) =
        let character = (state, data, characterData)
        data.BaseGeoRes + totalStats percentGeoRes character

    let totalDendroRes ((state, data, characterData): CharacterEntity) =
        let character = (state, data, characterData)
        data.BaseDendroRes + totalStats percentDendroRes character

    let totalPhysicalResIgnore (character: CharacterEntity) =
        totalStats percentPhysicalResIgnore character

    let totalPyroResIgnore (character: CharacterEntity) =
        totalStats percentPyroResIgnore character

    let totalHydroResIgnore (character: CharacterEntity) =
        totalStats percentHydroResIgnore character

    let totalElectroResIgnore (character: CharacterEntity) =
        totalStats percentElectroResIgnore character

    let totalCryoResIgnore (character: CharacterEntity) =
        totalStats percentCryoResIgnore character

    let totalAnemoResIgnore (character: CharacterEntity) =
        totalStats percentAnemoResIgnore character

    let totalGeoResIgnore (character: CharacterEntity) =
        totalStats percentGeoResIgnore character

    let totalDendroResIgnore (character: CharacterEntity) =
        totalStats percentDendroResIgnore character

    let totalFlatDamageBonus (character: CharacterEntity) =
        totalStats flatDamageBonus character

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

    let totalDefenseReduction entity =
        let f stat =
            match stat with
            | PercentDefenseReduction red -> red
            | _ -> 0.0
        let state =
            match entity with
            | EnemyEntity (s, _) -> s
            | CharacterEntity (s, _, _) -> s
        (0.0, state.Effects) ||> List.fold (fun s v -> s + f v)

    let totalDefenseIgnore entity =
        let f stat =
            match stat with
            | PercentDefenseIgnore red -> red
            | _ -> 0.0
        let state =
            match entity with
            | EnemyEntity (s, _) -> s
            | CharacterEntity (s, _, _) -> s
        (0.0, state.Effects) ||> List.fold (fun s v -> s + f v)

    let totalPhysicalRes entity =
        match entity with
        | EnemyEntity (_, data) -> data.BasePhysicalRes
        | CharacterEntity character -> CharacterEntity.totalPhysicalRes character

    let totalPyroRes entity =
        match entity with
        | EnemyEntity (_, data) -> data.BasePyroRes
        | CharacterEntity character -> CharacterEntity.totalPyroRes character

    let totalHydroRes entity =
        match entity with
        | EnemyEntity (_, data) -> data.BaseHydroRes
        | CharacterEntity character -> CharacterEntity.totalHydroRes character

    let totalElectroRes entity =
        match entity with
        | EnemyEntity (_, data) -> data.BaseElectroRes
        | CharacterEntity character -> CharacterEntity.totalElectroRes character

    let totalCryoRes entity =
        match entity with
        | EnemyEntity (_, data) -> data.BaseCryoRes
        | CharacterEntity character -> CharacterEntity.totalCryoRes character

    let totalAnemoRes entity =
        match entity with
        | EnemyEntity (_, data) -> data.BaseAnemoRes
        | CharacterEntity character -> CharacterEntity.totalAnemoRes character

    let totalGeoRes entity =
        match entity with
        | EnemyEntity (_, data) -> data.BaseGeoRes
        | CharacterEntity character -> CharacterEntity.totalGeoRes character

    let totalDendroRes entity =
        match entity with
        | EnemyEntity (_, data) -> data.BaseDendroRes
        | CharacterEntity character -> CharacterEntity.totalDendroRes character

    let totalPhysicalResIgnore entity =
        match entity with
        | EnemyEntity _ -> 0.0
        | CharacterEntity character -> CharacterEntity.totalPhysicalResIgnore character

    let totalPyroResIgnore entity =
        match entity with
        | EnemyEntity _ -> 0.0
        | CharacterEntity character -> CharacterEntity.totalPyroResIgnore character

    let totalHydroResIgnore entity =
        match entity with
        | EnemyEntity _ -> 0.0
        | CharacterEntity character -> CharacterEntity.totalHydroResIgnore character

    let totalElectroResIgnore entity =
        match entity with
        | EnemyEntity _ -> 0.0
        | CharacterEntity character -> CharacterEntity.totalElectroResIgnore character

    let totalCryoResIgnore entity =
        match entity with
        | EnemyEntity _ -> 0.0
        | CharacterEntity character -> CharacterEntity.totalCryoResIgnore character

    let totalAnemoResIgnore entity =
        match entity with
        | EnemyEntity _ -> 0.0
        | CharacterEntity character -> CharacterEntity.totalAnemoResIgnore character

    let totalGeoResIgnore entity =
        match entity with
        | EnemyEntity _ -> 0.0
        | CharacterEntity character -> CharacterEntity.totalGeoResIgnore character

    let totalDendroResIgnore entity =
        match entity with
        | EnemyEntity _ -> 0.0
        | CharacterEntity character -> CharacterEntity.totalDendroResIgnore character

    let totalFlatDamageBonus entity =
        match entity with
        | EnemyEntity _ -> 0.0
        | CharacterEntity character -> CharacterEntity.totalFlatDamageBonus character