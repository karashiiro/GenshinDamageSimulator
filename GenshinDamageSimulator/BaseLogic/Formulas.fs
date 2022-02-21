namespace GenshinDamageSimulator

open EntityLogic
open Resonance
open Data.ElementalCoefficients

// From https://genshin-impact.fandom.com/wiki/Damage

module Formulas =
    let getAmpifyingReactionMultiplier reaction =
        match reaction with
            | StrongVaporize | StrongMelt -> 2f
            | WeakVaporize | WeakMelt -> 1.5f
            | _ -> 1f

    let getTransformativeReactionMultiplier reaction =
        match reaction with
            | Superconduct -> 1f
            | Swirl -> 1.2f
            | ElectroCharged -> 2.4f
            | Shattered -> 3f
            | Overload -> 4f
            | _ -> 0f

    // HP formulas
    let calcTotalHp (b, c) =
        uint32 (float32 (b.BaseHp) * (1f + getBNpcStatPercent PercStat.Hp c)) + getBNpcStatFlat FlatStat.Hp c

    // Attack formulas
    let calcTotalAttack (b, c) party =
        uint32 (float32 (b.BaseAttack) * (1f + calcResonanceAttackPercent party * getBNpcStatPercent PercStat.Attack c)) + getBNpcStatFlat FlatStat.Attack c
        
    // Defense formulas
    let calcTotalDefense (b, c) =
        uint32 (float32 (b.BaseDefense) * (1f + getBNpcStatPercent PercStat.Defense c)) + getBNpcStatFlat FlatStat.Defense c

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