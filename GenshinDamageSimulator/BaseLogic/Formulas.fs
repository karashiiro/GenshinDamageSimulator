namespace GenshinDamageSimulator

open Entity
open Resonance
open Reactions
open ElementalCoefficients

// From https://genshin-impact.fandom.com/wiki/Damage

module Formulas =
    // HP formulas
    let calcTotalHp bNpc =
        uint32 (float32 (bNpc.BaseHp) * (1f + getBNpcStatPercent PercStat.Hp bNpc)) + getBNpcStatFlat FlatStat.Hp bNpc

    // Attack formulas
    let calcTotalAttack bNpc party =
        uint32 (float32 (bNpc.BaseAttack) * (1f + calcResonanceAttackPercent party * getBNpcStatPercent PercStat.Attack bNpc)) + getBNpcStatFlat FlatStat.Attack bNpc
        
    // Defense formulas
    let calcTotalDefense bNpc =
        uint32 (float32 (bNpc.BaseDefense) * (1f + getBNpcStatPercent PercStat.Defense bNpc)) + getBNpcStatFlat FlatStat.Defense bNpc

    // Elemental mastery formulas
    let calcTotalElementalMastery =
        getBNpcStatFlat FlatStat.ElementalMastery

    // Energy recharge formulas
    let calcTotalEnergyRecharge bNpc =
        1f + getBNpcStatPercent PercStat.EnergyRecharge bNpc

    // Crit rate formulas
    let calcTotalCriticalHit =
        getBNpcStatPercent PercStat.CriticalHit

    // Crit damage formulas
    let calcTotalCriticalDamage =
        getBNpcStatPercent PercStat.CriticalDamage

    // Physical damage bonus formulas
    let calcTotalPhysical =
        getBNpcStatPercent PercStat.Physical

    // Pyro damage bonus formulas
    let calcTotalPyro =
        getBNpcStatPercent PercStat.Pyro

    // Hydro damage bonus formulas
    let calcTotalHydro =
        getBNpcStatPercent PercStat.Hydro

    // Electro damage bonus formulas
    let calcTotalElectro =
        getBNpcStatPercent PercStat.Electro

    // Cryo damage bonus formulas
    let calcTotalCryo =
        getBNpcStatPercent PercStat.Cryo

    // Anemo damage bonus formulas
    let calcTotalAnemo =
        getBNpcStatPercent PercStat.Anemo

    // Geo damage bonus formulas
    let calcTotalGeo =
        getBNpcStatPercent PercStat.Geo

    // Dendro damage bonus formulas
    let calcTotalDendro =
        getBNpcStatPercent PercStat.Dendro

    // Physical resistance formulas
    let calcTotalPhysicalRes =
        getBNpcStatPercent PercStat.PhysicalRes

    // Pyro resistance formulas
    let calcTotalPyroRes =
        getBNpcStatPercent PercStat.PyroRes

    // Hydro resistance formulas
    let calcTotalHydroRes =
        getBNpcStatPercent PercStat.HydroRes

    // Electro resistance formulas
    let calcTotalElectroRes =
        getBNpcStatPercent PercStat.ElectroRes

    // Cryo resistance formulas
    let calcTotalCryoRes =
        getBNpcStatPercent PercStat.CryoRes

    // Anemo resistance formulas
    let calcTotalAnemoRes =
        getBNpcStatPercent PercStat.AnemoRes

    // Geo resistance formulas
    let calcTotalGeoRes =
        getBNpcStatPercent PercStat.GeoRes

    // Dendro resistance formulas
    let calcTotalDendroRes =
        getBNpcStatPercent PercStat.DendroRes

    // Outgoing damage formulas
    let calcAmplifyingBonus em =
        2.78f * (float32 em / float32 (em + 1400u))

    let calcAmplifyingMultiplier reaction amplifyingBonus reactionBonus =
        getAmpifyingReactionMultiplier reaction * (1f + amplifyingBonus + reactionBonus)

    let calcTransformativeBonus em =
        16f * (float32 em / float32 (em + 2000u))

    let calcTransformativeDamageCharacter reaction characterLevel transformativeBonus reactionBonus =
        let levelMult = getCharacterLevelMultiplier characterLevel
        uint32 (levelMult * getTransformativeReactionMultiplier reaction * (1f + transformativeBonus + reactionBonus))

    let calcTransformativeDamageEnemy reaction enemyLevel transformativeBonus reactionBonus =
        let levelMult = getEnemyLevelMultiplier enemyLevel
        uint32 (levelMult * getTransformativeReactionMultiplier reaction * (1f + transformativeBonus + reactionBonus))

    let calcAverageCritMultiplier critRate critDamage =
        1f + (min (critRate) 0f) * critDamage

    let calcCritMultiplier critDamage =
        1f + critDamage

    let calcOutgoingDamage (abilityStat: uint32) abilityMult bonusFlat bonusMult =
        uint32 (float32 (uint32 (float32 (abilityStat) * abilityMult) + bonusFlat) * bonusMult)

    // Incoming damage formulas
    let calcEnemyDefense level =
        5u * level + 500u

    let calcBaseDefenseMultiplier defense attackerLevel =
        1f - (float32 defense / float32 (defense + 5u * attackerLevel + 500u))

    let calcLevelDiffDefenseMultiplier attackerLevel defenderLevel defReductionPerc defIgnoredPerc =
        float32 (attackerLevel + 100u) / (float32 (attackerLevel + 100u) + float32 (defenderLevel + 100u) * (1f - defReductionPerc) * (1f - defIgnoredPerc))

    let calcDefenseMultiplier defense attackerLevel defenderLevel defReductionPerc defIgnoredPerc =
        (calcBaseDefenseMultiplier defense attackerLevel) * (calcLevelDiffDefenseMultiplier attackerLevel defenderLevel defReductionPerc defIgnoredPerc)

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