namespace GenshinDamageSimulator

open StatTypes
open EntityTypes
open Entity
open Party
open Reactions
open ElementalCoefficients

// From https://genshin-impact.fandom.com/wiki/Damage

module Formulas =
    // HP formulas
    let calcTotalHp bNpc =
        uint32 (float32 (bNpc.BaseHp) * (1f + getBNpcStatPercent bNpc PercStatType.Hp)) + getBNpcStatFlat bNpc FlatStatType.Hp

    // Attack formulas
    let calcTotalAttack bNpc party =
        uint32 (float32 (bNpc.BaseAttack) * (1f + calcResonanceAttackPercent party * getBNpcStatPercent bNpc PercStatType.Attack)) + getBNpcStatFlat bNpc FlatStatType.Attack
        
    // Defense formulas
    let calcTotalDefense bNpc =
        uint32 (float32 (bNpc.BaseDefense) * (1f + getBNpcStatPercent bNpc PercStatType.Defense)) + getBNpcStatFlat bNpc FlatStatType.Defense

    // Elemental mastery formulas
    let calcTotalElementalMastery bNpc =
        getBNpcStatFlat bNpc FlatStatType.ElementalMastery

    // Energy recharge formulas
    let calcTotalEnergyRecharge bNpc =
        1f + getBNpcStatPercent bNpc PercStatType.EnergyRecharge

    // Crit rate formulas
    let calcTotalCriticalHit bNpc =
        getBNpcStatPercent bNpc PercStatType.CriticalHit

    // Crit damage formulas
    let calcTotalCriticalDamage bNpc =
        getBNpcStatPercent bNpc PercStatType.CriticalDamage

    // Physical damage bonus formulas
    let calcTotalPhysical bNpc =
        getBNpcStatPercent bNpc PercStatType.Physical

    // Pyro damage bonus formulas
    let calcTotalPyro bNpc =
        getBNpcStatPercent bNpc PercStatType.Pyro

    // Hydro damage bonus formulas
    let calcTotalHydro bNpc =
        getBNpcStatPercent bNpc PercStatType.Hydro

    // Electro damage bonus formulas
    let calcTotalElectro bNpc =
        getBNpcStatPercent bNpc PercStatType.Electro

    // Cryo damage bonus formulas
    let calcTotalCryo bNpc =
        getBNpcStatPercent bNpc PercStatType.Cryo

    // Anemo damage bonus formulas
    let calcTotalAnemo bNpc =
        getBNpcStatPercent bNpc PercStatType.Anemo

    // Geo damage bonus formulas
    let calcTotalGeo bNpc =
        getBNpcStatPercent bNpc PercStatType.Geo

    // Dendro damage bonus formulas
    let calcTotalDendro bNpc =
        getBNpcStatPercent bNpc PercStatType.Dendro

    // Physical resistance formulas
    let calcTotalPhysicalRes bNpc =
        getBNpcStatPercent bNpc PercStatType.PhysicalRes

    // Pyro resistance formulas
    let calcTotalPyroRes bNpc =
        getBNpcStatPercent bNpc PercStatType.PyroRes

    // Hydro resistance formulas
    let calcTotalHydroRes bNpc =
        getBNpcStatPercent bNpc PercStatType.HydroRes

    // Electro resistance formulas
    let calcTotalElectroRes bNpc =
        getBNpcStatPercent bNpc PercStatType.ElectroRes

    // Cryo resistance formulas
    let calcTotalCryoRes bNpc =
        getBNpcStatPercent bNpc PercStatType.CryoRes

    // Anemo resistance formulas
    let calcTotalAnemoRes bNpc =
        getBNpcStatPercent bNpc PercStatType.AnemoRes

    // Geo resistance formulas
    let calcTotalGeoRes bNpc =
        getBNpcStatPercent bNpc PercStatType.GeoRes

    // Dendro resistance formulas
    let calcTotalDendroRes bNpc =
        getBNpcStatPercent bNpc PercStatType.DendroRes

    // Outgoing damage formulas
    let calcAmplifyingBonus em =
        2.78f * (float32 em / float32 (em + 1400u))

    let calcAmplifyingMultiplier reaction amplifyingBonus reactionBonus =
        getAmpifyingReactionMultiplier reaction * (1f + amplifyingBonus + reactionBonus)

    let calcTransformativeBonus em =
        16f * (float32 em / float32 (em + 2000u))

    let calcTransformativeDamageCharacter reaction characterLevel transformativeBonus reactionBonus =
        let levelMult = Map.tryFind characterLevel characterLevelMultipliers
        match levelMult with
            | Some x -> uint32 (x * getTransformativeReactionMultiplier reaction * (1f + transformativeBonus + reactionBonus))
            | None -> 0u

    let calcTransformativeDamageEnemy reaction enemyLevel transformativeBonus reactionBonus =
        let levelMult = Map.tryFind enemyLevel enemyLevelMultipliers
        match levelMult with
            | Some x -> uint32 (x * getTransformativeReactionMultiplier reaction * (1f + transformativeBonus + reactionBonus))
            | None -> 0u

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

    let calcIncomingTransformativeDamage (baseDamage: uint32) resMult =
        uint32 (float32 (baseDamage) * resMult)

    let calcIncomingDamage amplifyingMult (baseDamage: uint32) defMult resMult dmgReductionMult =
        uint32 (amplifyingMult * float32 (uint32 (float32 (baseDamage) * defMult * resMult * dmgReductionMult)))