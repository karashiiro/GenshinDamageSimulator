namespace GenshinDamageSimulator

open EntityTypes
open Entity
open Party

// From https://genshin-impact.fandom.com/wiki/Damage

module Formulas =
    // HP formulas
    let calcTotalHpMultiplier bNpc =
        1f + calcBnpcHpPercent bNpc + calcWeaponHpPercent bNpc.Weapon + calcArtifactsHpPercent bNpc.Artifacts

    let calcTotalHpFlat bNpc =
        calcBnpcHpFlat bNpc + calcWeaponHpFlat bNpc.Weapon + calcArtifactsHpFlat bNpc.Artifacts

    let calcTotalHp bNpc =
        uint32 (float32 bNpc.BaseHp * calcTotalHpMultiplier bNpc) + calcTotalHpFlat bNpc

    // Attack formulas
    let calcTotalAttackMultiplier bNpc party =
        1f + calcWeaponAttackPercent bNpc.Weapon + calcArtifactsAttackPercent bNpc.Artifacts + calcResonanceAttackPercent party

    let calcTotalAttackFlat bNpc =
        calcBnpcAttackFlat bNpc + calcWeaponAttackFlat bNpc.Weapon + calcArtifactsAttackFlat bNpc.Artifacts

    let calcBaseAttack bNpc =
        bNpc.Weapon.Attack + bNpc.BaseAttack

    let calcTotalAttack bNpc party =
        uint32 (float32 (calcBaseAttack bNpc) * calcTotalAttackMultiplier bNpc party) + calcTotalAttackFlat bNpc
        
    // Defense formulas
    let calcTotalDefenseMultiplier bNpc =
        1f + calcWeaponDefensePercent bNpc.Weapon + calcArtifactsDefensePercent bNpc.Artifacts

    let calcTotalDefenseFlat bNpc =
        calcBnpcDefenseFlat bNpc + calcWeaponDefenseFlat bNpc.Weapon + calcArtifactsDefenseFlat bNpc.Artifacts

    let calcTotalDefense bNpc =
        uint32 (float32 bNpc.BaseDefense * calcTotalDefenseMultiplier bNpc) + calcTotalDefenseFlat bNpc

    // Elemental mastery formulas
    let calcTotalElementalMastery bNpc =
        calcBnpcElementalMasteryFlat bNpc + calcWeaponElementalMasteryFlat bNpc.Weapon + calcArtifactsElementalMasteryFlat bNpc.Artifacts

    // Energy recharge formulas
    let calcTotalEnergyRecharge bNpc =
        1f + calcBnpcEnergyRechargePercent bNpc + calcWeaponEnergyRechargePercent bNpc.Weapon + calcArtifactsEnergyRechargePercent bNpc.Artifacts

    // Crit rate formulas
    let calcTotalCriticalHit bNpc =
        calcBnpcCriticalHitPercent bNpc + calcWeaponCriticalHitPercent bNpc.Weapon + calcArtifactsCriticalHitPercent bNpc.Artifacts

    // Crit damage formulas
    let calcTotalCriticalDamage bNpc =
        calcBnpcCriticalDamagePercent bNpc + calcWeaponCriticalDamagePercent bNpc.Weapon + calcArtifactsCriticalDamagePercent bNpc.Artifacts

    // Physical damage bonus formulas
    let calcTotalPhysical bNpc =
        calcBnpcPhysicalPercent bNpc + calcWeaponPhysicalPercent bNpc.Weapon + calcArtifactsPhysicalPercent bNpc.Artifacts

    // Pyro damage bonus formulas
    let calcTotalPyro bNpc =
        calcBnpcPyroPercent bNpc + calcWeaponPyroPercent bNpc.Weapon + calcArtifactsPyroPercent bNpc.Artifacts

    // Hydro damage bonus formulas
    let calcTotalHydro bNpc =
        calcBnpcHydroPercent bNpc + calcWeaponHydroPercent bNpc.Weapon + calcArtifactsHydroPercent bNpc.Artifacts

    // Electro damage bonus formulas
    let calcTotalElectro bNpc =
        calcBnpcElectroPercent bNpc + calcWeaponElectroPercent bNpc.Weapon + calcArtifactsElectroPercent bNpc.Artifacts

    // Cryo damage bonus formulas
    let calcTotalCryo bNpc =
        calcBnpcCryoPercent bNpc + calcWeaponCryoPercent bNpc.Weapon + calcArtifactsCryoPercent bNpc.Artifacts

    // Anemo damage bonus formulas
    let calcTotalAnemo bNpc =
        calcBnpcAnemoPercent bNpc + calcWeaponAnemoPercent bNpc.Weapon + calcArtifactsAnemoPercent bNpc.Artifacts

    // Geo damage bonus formulas
    let calcTotalGeo bNpc =
        calcBnpcGeoPercent bNpc + calcWeaponGeoPercent bNpc.Weapon + calcArtifactsGeoPercent bNpc.Artifacts

    // Dendro damage bonus formulas
    let calcTotalDendro bNpc =
        calcBnpcDendroPercent bNpc + calcWeaponDendroPercent bNpc.Weapon + calcArtifactsDendroPercent bNpc.Artifacts

    // Physical resistance formulas
    let calcTotalPhysicalRes bNpc =
        calcBnpcPhysicalResPercent bNpc + calcWeaponPhysicalResPercent bNpc.Weapon + calcArtifactsPhysicalResPercent bNpc.Artifacts

    // Pyro resistance formulas
    let calcTotalPyroRes bNpc =
        calcBnpcPyroResPercent bNpc + calcWeaponPyroResPercent bNpc.Weapon + calcArtifactsPyroResPercent bNpc.Artifacts

    // Hydro resistance formulas
    let calcTotalHydroRes bNpc =
        calcBnpcHydroResPercent bNpc + calcWeaponHydroResPercent bNpc.Weapon + calcArtifactsHydroResPercent bNpc.Artifacts

    // Electro resistance formulas
    let calcTotalElectroRes bNpc =
        calcBnpcElectroResPercent bNpc + calcWeaponElectroResPercent bNpc.Weapon + calcArtifactsElectroResPercent bNpc.Artifacts

    // Cryo resistance formulas
    let calcTotalCryoRes bNpc =
        calcBnpcCryoResPercent bNpc + calcWeaponCryoResPercent bNpc.Weapon + calcArtifactsCryoResPercent bNpc.Artifacts

    // Anemo resistance formulas
    let calcTotalAnemoRes bNpc =
        calcBnpcAnemoResPercent bNpc + calcWeaponAnemoResPercent bNpc.Weapon + calcArtifactsAnemoResPercent bNpc.Artifacts

    // Geo resistance formulas
    let calcTotalGeoRes bNpc =
        calcBnpcGeoResPercent bNpc + calcWeaponGeoResPercent bNpc.Weapon + calcArtifactsGeoResPercent bNpc.Artifacts

    // Dendro resistance formulas
    let calcTotalDendroRes bNpc =
        calcBnpcDendroResPercent bNpc + calcWeaponDendroResPercent bNpc.Weapon + calcArtifactsDendroResPercent bNpc.Artifacts

    // Outgoing damage formulas
    let calcAmplifyingBonus (em: uint32) =
        2.78f * (float32 em / float32 (em + 1400u))

    let calcAmplifyingMultiplier reactionMult amplifyingBonus reactionBonus =
        reactionMult * (1f + amplifyingBonus + reactionBonus)

    // TODO: Transformative reactions
    // TODO: Swirl reactions

    let calcAverageCritMultiplier critRate critDamage =
        1f + critRate * critDamage

    let calcOutgoingDamage (abilityStat: uint32) abilityMult bonusFlat bonusMult =
        uint32 (float32 (uint32 (float32 (abilityStat) * abilityMult) + bonusFlat) * bonusMult)

    // Incoming damage formulas
    let calcEnemyDefense level =
        5u * level + 500u

    let calcBaseDefenseMultiplier defense attackerLevel =
        1f - (float32 defense / float32 (defense + 5u * attackerLevel + 500u))

    let calcLevelDiffDefenseMultiplier attackerLevel enemyLevel defReductionPerc defIgnoredPerc =
        float32 (attackerLevel + 100u) / (float32 (attackerLevel + 100u) + float32 (enemyLevel + 100u) * (1f - defReductionPerc) * (1f - defIgnoredPerc))

    let calcDefenseMultiplier defense attackerLevel enemyLevel defReductionPerc defIgnoredPerc =
        (calcBaseDefenseMultiplier defense attackerLevel) * (calcLevelDiffDefenseMultiplier attackerLevel enemyLevel defReductionPerc defIgnoredPerc)

    let calcResMultiplier baseResPerc resBonusPerc resReductionPerc =
        let resPerc = baseResPerc + resBonusPerc - resReductionPerc
        if resPerc < 0f then
            1f - resPerc / 2f
        elif resPerc < 0.75f then
            1f - resPerc
        else
            1f / (4f * resPerc + 1f)

    let calcDamageReductionMultiplier dmgReductionPerc =
        1f - dmgReductionPerc

    let calcIncomingDamage amplifyingMult (baseDamage: uint32) defMult resMult dmgReductionMult =
        uint32 (amplifyingMult * float32 (uint32 (float32 (baseDamage) * defMult * resMult * dmgReductionMult)))