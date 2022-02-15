namespace GenshinDamageSimulator

open EntityTypes
open Stat

module Entity =
    // Flat stats: HP
    let calcBnpcHpFlat (bNpc: BattleNpc) =
        calcModifierHpFlat bNpc.MainStat

    let calcWeaponHpFlat (weapon: Weapon) =
        calcModifierHpFlat weapon.MainStat

    let calcArtifactsMainHpFlat (artifacts: Artifact seq) =
        artifacts
        |> Seq.map (fun x -> calcModifierHpFlat x.MainStat)
        |> Seq.sum

    let calcArtifactsSubHpFlat (artifacts: Artifact seq) =
        artifacts
        |> Seq.map (fun x -> x.StatLines |> Seq.map calcModifierHpFlat |> Seq.sum)
        |> Seq.sum

    let calcArtifactsHpFlat (artifacts: Artifact seq) =
        calcArtifactsMainHpFlat artifacts + calcArtifactsSubHpFlat artifacts

    // Flat stats: Attack
    let calcBnpcAttackFlat (bNpc: BattleNpc) =
        calcModifierAttackFlat bNpc.MainStat

    let calcWeaponAttackFlat (weapon: Weapon) =
        calcModifierAttackFlat weapon.MainStat

    let calcArtifactsMainAttackFlat (artifacts: Artifact seq) =
        artifacts
        |> Seq.map (fun x -> calcModifierAttackFlat x.MainStat)
        |> Seq.sum

    let calcArtifactsSubAttackFlat (artifacts: Artifact seq) =
        artifacts
        |> Seq.map (fun x -> x.StatLines |> Seq.map calcModifierAttackFlat |> Seq.sum)
        |> Seq.sum

    let calcArtifactsAttackFlat (artifacts: Artifact seq) =
        calcArtifactsMainAttackFlat artifacts + calcArtifactsSubAttackFlat artifacts

    // Flat stats: Defense
    let calcBnpcDefenseFlat (bNpc: BattleNpc) =
        calcModifierDefenseFlat bNpc.MainStat

    let calcWeaponDefenseFlat (weapon: Weapon) =
        calcModifierDefenseFlat weapon.MainStat

    let calcArtifactsMainDefenseFlat (artifacts: Artifact seq) =
        artifacts
        |> Seq.map (fun x -> calcModifierDefenseFlat x.MainStat)
        |> Seq.sum

    let calcArtifactsSubDefenseFlat (artifacts: Artifact seq) =
        artifacts
        |> Seq.map (fun x -> x.StatLines |> Seq.map calcModifierDefenseFlat |> Seq.sum)
        |> Seq.sum

    let calcArtifactsDefenseFlat (artifacts: Artifact seq) =
        calcArtifactsMainDefenseFlat artifacts + calcArtifactsSubDefenseFlat artifacts

    // Flat stats: Elemental mastery
    let calcBnpcElementalMasteryFlat (bNpc: BattleNpc) =
        calcModifierElementalMasteryFlat bNpc.MainStat

    let calcWeaponElementalMasteryFlat (weapon: Weapon) =
        calcModifierElementalMasteryFlat weapon.MainStat

    let calcArtifactsMainElementalMasteryFlat (artifacts: Artifact seq) =
        artifacts
        |> Seq.map (fun x -> calcModifierElementalMasteryFlat x.MainStat)
        |> Seq.sum

    let calcArtifactsSubElementalMasteryFlat (artifacts: Artifact seq) =
        artifacts
        |> Seq.map (fun x -> x.StatLines |> Seq.map calcModifierElementalMasteryFlat |> Seq.sum)
        |> Seq.sum

    let calcArtifactsElementalMasteryFlat (artifacts: Artifact seq) =
        calcArtifactsMainElementalMasteryFlat artifacts + calcArtifactsSubElementalMasteryFlat artifacts

    // Percent stats: HP
    let calcBnpcHpPercent (bNpc: BattleNpc) =
        calcModifierHpPercent bNpc.MainStat

    let calcWeaponHpPercent (weapon: Weapon) =
        calcModifierHpPercent weapon.MainStat

    let calcArtifactsMainHpPercent (artifacts: Artifact seq) =
        artifacts
        |> Seq.map (fun x -> calcModifierHpPercent x.MainStat)
        |> Seq.sum

    let calcArtifactsSubHpPercent (artifacts: Artifact seq) =
        artifacts
        |> Seq.map (fun x -> x.StatLines |> Seq.map calcModifierHpPercent |> Seq.sum)
        |> Seq.sum

    let calcArtifactsHpPercent (artifacts: Artifact seq) =
        calcArtifactsMainHpPercent artifacts + calcArtifactsSubHpPercent artifacts

    // Percent stats: Attack
    let calcBnpcAttackPercent (bNpc: BattleNpc) =
        calcModifierAttackPercent bNpc.MainStat

    let calcWeaponAttackPercent (weapon: Weapon) =
        calcModifierAttackPercent weapon.MainStat

    let calcArtifactsMainAttackPercent (artifacts: Artifact seq) =
        artifacts
        |> Seq.map (fun x -> calcModifierAttackPercent x.MainStat)
        |> Seq.sum

    let calcArtifactsSubAttackPercent (artifacts: Artifact seq) =
        artifacts
        |> Seq.map (fun x -> x.StatLines |> Seq.map calcModifierAttackPercent |> Seq.sum)
        |> Seq.sum

    let calcArtifactsAttackPercent (artifacts: Artifact seq) =
        calcArtifactsMainAttackPercent artifacts + calcArtifactsSubAttackPercent artifacts

    // Percent stats: Defense
    let calcBnpcDefensePercent (bNpc: BattleNpc) =
        calcModifierDefensePercent bNpc.MainStat

    let calcWeaponDefensePercent (weapon: Weapon) =
        calcModifierDefensePercent weapon.MainStat

    let calcArtifactsMainDefensePercent (artifacts: Artifact seq) =
        artifacts
        |> Seq.map (fun x -> calcModifierDefensePercent x.MainStat)
        |> Seq.sum

    let calcArtifactsSubDefensePercent (artifacts: Artifact seq) =
        artifacts
        |> Seq.map (fun x -> x.StatLines |> Seq.map calcModifierDefensePercent |> Seq.sum)
        |> Seq.sum

    let calcArtifactsDefensePercent (artifacts: Artifact seq) =
        calcArtifactsMainDefensePercent artifacts + calcArtifactsSubDefensePercent artifacts

    // Percent stats: Energy Recharge
    let calcBnpcEnergyRechargePercent (bNpc: BattleNpc) =
        calcModifierEnergyRechargePercent bNpc.MainStat

    let calcWeaponEnergyRechargePercent (weapon: Weapon) =
        calcModifierEnergyRechargePercent weapon.MainStat

    let calcArtifactsMainEnergyRechargePercent (artifacts: Artifact seq) =
        artifacts
        |> Seq.map (fun x -> calcModifierEnergyRechargePercent x.MainStat)
        |> Seq.sum

    let calcArtifactsSubEnergyRechargePercent (artifacts: Artifact seq) =
        artifacts
        |> Seq.map (fun x -> x.StatLines |> Seq.map calcModifierEnergyRechargePercent |> Seq.sum)
        |> Seq.sum

    let calcArtifactsEnergyRechargePercent (artifacts: Artifact seq) =
        calcArtifactsMainEnergyRechargePercent artifacts + calcArtifactsSubEnergyRechargePercent artifacts

    // Percent stats: Critical Hit
    let calcBnpcCriticalHitPercent (bNpc: BattleNpc) =
        calcModifierCriticalHitPercent bNpc.MainStat

    let calcWeaponCriticalHitPercent (weapon: Weapon) =
        calcModifierCriticalHitPercent weapon.MainStat

    let calcArtifactsMainCriticalHitPercent (artifacts: Artifact seq) =
        artifacts
        |> Seq.map (fun x -> calcModifierCriticalHitPercent x.MainStat)
        |> Seq.sum

    let calcArtifactsSubCriticalHitPercent (artifacts: Artifact seq) =
        artifacts
        |> Seq.map (fun x -> x.StatLines |> Seq.map calcModifierCriticalHitPercent |> Seq.sum)
        |> Seq.sum

    let calcArtifactsCriticalHitPercent (artifacts: Artifact seq) =
        calcArtifactsMainCriticalHitPercent artifacts + calcArtifactsSubCriticalHitPercent artifacts

    // Percent stats: Critical Damage
    let calcBnpcCriticalDamagePercent (bNpc: BattleNpc) =
        calcModifierCriticalDamagePercent bNpc.MainStat

    let calcWeaponCriticalDamagePercent (weapon: Weapon) =
        calcModifierCriticalDamagePercent weapon.MainStat

    let calcArtifactsMainCriticalDamagePercent (artifacts: Artifact seq) =
        artifacts
        |> Seq.map (fun x -> calcModifierCriticalDamagePercent x.MainStat)
        |> Seq.sum

    let calcArtifactsSubCriticalDamagePercent (artifacts: Artifact seq) =
        artifacts
        |> Seq.map (fun x -> x.StatLines |> Seq.map calcModifierCriticalDamagePercent |> Seq.sum)
        |> Seq.sum

    let calcArtifactsCriticalDamagePercent (artifacts: Artifact seq) =
        calcArtifactsMainCriticalDamagePercent artifacts + calcArtifactsSubCriticalDamagePercent artifacts

    // Percent stats: Physical Damage Bonus
    let calcBnpcPhysicalPercent (bNpc: BattleNpc) =
        calcModifierPhysicalPercent bNpc.MainStat

    let calcWeaponPhysicalPercent (weapon: Weapon) =
        calcModifierPhysicalPercent weapon.MainStat

    let calcArtifactsMainPhysicalPercent (artifacts: Artifact seq) =
        artifacts
        |> Seq.map (fun x -> calcModifierPhysicalPercent x.MainStat)
        |> Seq.sum

    let calcArtifactsPhysicalPercent (artifacts: Artifact seq) =
        calcArtifactsMainPhysicalPercent artifacts

    // Percent stats: Pyro Damage Bonus
    let calcBnpcPyroPercent (bNpc: BattleNpc) =
        calcModifierPyroPercent bNpc.MainStat

    let calcWeaponPyroPercent (weapon: Weapon) =
        calcModifierPyroPercent weapon.MainStat

    let calcArtifactsMainPyroPercent (artifacts: Artifact seq) =
        artifacts
        |> Seq.map (fun x -> calcModifierPyroPercent x.MainStat)
        |> Seq.sum

    let calcArtifactsPyroPercent (artifacts: Artifact seq) =
        calcArtifactsMainPyroPercent artifacts

    // Percent stats: Hydro Damage Bonus
    let calcBnpcHydroPercent (bNpc: BattleNpc) =
        calcModifierHydroPercent bNpc.MainStat

    let calcWeaponHydroPercent (weapon: Weapon) =
        calcModifierHydroPercent weapon.MainStat

    let calcArtifactsMainHydroPercent (artifacts: Artifact seq) =
        artifacts
        |> Seq.map (fun x -> calcModifierHydroPercent x.MainStat)
        |> Seq.sum

    let calcArtifactsHydroPercent (artifacts: Artifact seq) =
        calcArtifactsMainHydroPercent artifacts

    // Percent stats: Electro Damage Bonus
    let calcBnpcElectroPercent (bNpc: BattleNpc) =
        calcModifierElectroPercent bNpc.MainStat

    let calcWeaponElectroPercent (weapon: Weapon) =
        calcModifierElectroPercent weapon.MainStat

    let calcArtifactsMainElectroPercent (artifacts: Artifact seq) =
        artifacts
        |> Seq.map (fun x -> calcModifierElectroPercent x.MainStat)
        |> Seq.sum

    let calcArtifactsElectroPercent (artifacts: Artifact seq) =
        calcArtifactsMainElectroPercent artifacts

    // Percent stats: Cryo Damage Bonus
    let calcBnpcCryoPercent (bNpc: BattleNpc) =
        calcModifierCryoPercent bNpc.MainStat

    let calcWeaponCryoPercent (weapon: Weapon) =
        calcModifierCryoPercent weapon.MainStat

    let calcArtifactsMainCryoPercent (artifacts: Artifact seq) =
        artifacts
        |> Seq.map (fun x -> calcModifierCryoPercent x.MainStat)
        |> Seq.sum

    let calcArtifactsCryoPercent (artifacts: Artifact seq) =
        calcArtifactsMainCryoPercent artifacts

    // Percent stats: Anemo Damage Bonus
    let calcBnpcAnemoPercent (bNpc: BattleNpc) =
        calcModifierAnemoPercent bNpc.MainStat

    let calcWeaponAnemoPercent (weapon: Weapon) =
        calcModifierAnemoPercent weapon.MainStat

    let calcArtifactsMainAnemoPercent (artifacts: Artifact seq) =
        artifacts
        |> Seq.map (fun x -> calcModifierAnemoPercent x.MainStat)
        |> Seq.sum

    let calcArtifactsAnemoPercent (artifacts: Artifact seq) =
        calcArtifactsMainAnemoPercent artifacts

    // Percent stats: Geo Damage Bonus
    let calcBnpcGeoPercent (bNpc: BattleNpc) =
        calcModifierGeoPercent bNpc.MainStat

    let calcWeaponGeoPercent (weapon: Weapon) =
        calcModifierGeoPercent weapon.MainStat

    let calcArtifactsMainGeoPercent (artifacts: Artifact seq) =
        artifacts
        |> Seq.map (fun x -> calcModifierGeoPercent x.MainStat)
        |> Seq.sum

    let calcArtifactsGeoPercent (artifacts: Artifact seq) =
        calcArtifactsMainGeoPercent artifacts

    // Percent stats: Dendro Damage Bonus
    let calcBnpcDendroPercent (bNpc: BattleNpc) =
        calcModifierDendroPercent bNpc.MainStat

    let calcWeaponDendroPercent (weapon: Weapon) =
        calcModifierDendroPercent weapon.MainStat

    let calcArtifactsMainDendroPercent (artifacts: Artifact seq) =
        artifacts
        |> Seq.map (fun x -> calcModifierDendroPercent x.MainStat)
        |> Seq.sum

    let calcArtifactsDendroPercent (artifacts: Artifact seq) =
        calcArtifactsMainDendroPercent artifacts

    // Percent stats: Physical Damage Reistance
    let calcBnpcPhysicalResPercent (bNpc: BattleNpc) =
        calcModifierPhysicalResPercent bNpc.MainStat

    let calcWeaponPhysicalResPercent (weapon: Weapon) =
        calcModifierPhysicalResPercent weapon.MainStat

    let calcArtifactsMainPhysicalResPercent (artifacts: Artifact seq) =
        artifacts
        |> Seq.map (fun x -> calcModifierPhysicalResPercent x.MainStat)
        |> Seq.sum

    let calcArtifactsPhysicalResPercent (artifacts: Artifact seq) =
        calcArtifactsMainPhysicalResPercent artifacts

    // Percent stats: Pyro Damage Reistance
    let calcBnpcPyroResPercent (bNpc: BattleNpc) =
        calcModifierPyroResPercent bNpc.MainStat

    let calcWeaponPyroResPercent (weapon: Weapon) =
        calcModifierPyroResPercent weapon.MainStat

    let calcArtifactsMainPyroResPercent (artifacts: Artifact seq) =
        artifacts
        |> Seq.map (fun x -> calcModifierPyroResPercent x.MainStat)
        |> Seq.sum

    let calcArtifactsPyroResPercent (artifacts: Artifact seq) =
        calcArtifactsMainPyroResPercent artifacts

    // Percent stats: Hydro Damage Reistance
    let calcBnpcHydroResPercent (bNpc: BattleNpc) =
        calcModifierHydroResPercent bNpc.MainStat

    let calcWeaponHydroResPercent (weapon: Weapon) =
        calcModifierHydroResPercent weapon.MainStat

    let calcArtifactsMainHydroResPercent (artifacts: Artifact seq) =
        artifacts
        |> Seq.map (fun x -> calcModifierHydroResPercent x.MainStat)
        |> Seq.sum

    let calcArtifactsHydroResPercent (artifacts: Artifact seq) =
        calcArtifactsMainHydroResPercent artifacts

    // Percent stats: Electro Damage Reistance
    let calcBnpcElectroResPercent (bNpc: BattleNpc) =
        calcModifierElectroResPercent bNpc.MainStat

    let calcWeaponElectroResPercent (weapon: Weapon) =
        calcModifierElectroResPercent weapon.MainStat

    let calcArtifactsMainElectroResPercent (artifacts: Artifact seq) =
        artifacts
        |> Seq.map (fun x -> calcModifierElectroResPercent x.MainStat)
        |> Seq.sum

    let calcArtifactsElectroResPercent (artifacts: Artifact seq) =
        calcArtifactsMainElectroResPercent artifacts

    // Percent stats: Cryo Damage Reistance
    let calcBnpcCryoResPercent (bNpc: BattleNpc) =
        calcModifierCryoResPercent bNpc.MainStat

    let calcWeaponCryoResPercent (weapon: Weapon) =
        calcModifierCryoResPercent weapon.MainStat

    let calcArtifactsMainCryoResPercent (artifacts: Artifact seq) =
        artifacts
        |> Seq.map (fun x -> calcModifierCryoResPercent x.MainStat)
        |> Seq.sum

    let calcArtifactsCryoResPercent (artifacts: Artifact seq) =
        calcArtifactsMainCryoResPercent artifacts

    // Percent stats: Anemo Damage Reistance
    let calcBnpcAnemoResPercent (bNpc: BattleNpc) =
        calcModifierAnemoResPercent bNpc.MainStat

    let calcWeaponAnemoResPercent (weapon: Weapon) =
        calcModifierAnemoResPercent weapon.MainStat

    let calcArtifactsMainAnemoResPercent (artifacts: Artifact seq) =
        artifacts
        |> Seq.map (fun x -> calcModifierAnemoResPercent x.MainStat)
        |> Seq.sum

    let calcArtifactsAnemoResPercent (artifacts: Artifact seq) =
        calcArtifactsMainAnemoResPercent artifacts

    // Percent stats: Geo Damage Reistance
    let calcBnpcGeoResPercent (bNpc: BattleNpc) =
        calcModifierGeoResPercent bNpc.MainStat

    let calcWeaponGeoResPercent (weapon: Weapon) =
        calcModifierGeoResPercent weapon.MainStat

    let calcArtifactsMainGeoResPercent (artifacts: Artifact seq) =
        artifacts
        |> Seq.map (fun x -> calcModifierGeoResPercent x.MainStat)
        |> Seq.sum

    let calcArtifactsGeoResPercent (artifacts: Artifact seq) =
        calcArtifactsMainGeoResPercent artifacts

    // Percent stats: Dendro Damage Reistance
    let calcBnpcDendroResPercent (bNpc: BattleNpc) =
        calcModifierDendroResPercent bNpc.MainStat

    let calcWeaponDendroResPercent (weapon: Weapon) =
        calcModifierDendroResPercent weapon.MainStat

    let calcArtifactsMainDendroResPercent (artifacts: Artifact seq) =
        artifacts
        |> Seq.map (fun x -> calcModifierDendroResPercent x.MainStat)
        |> Seq.sum

    let calcArtifactsDendroResPercent (artifacts: Artifact seq) =
        calcArtifactsMainDendroResPercent artifacts