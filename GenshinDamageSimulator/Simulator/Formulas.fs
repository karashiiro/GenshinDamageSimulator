namespace GenshinDamageSimulator

open GenshinDamageSimulator.Data

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
            | ElectroCharged _ -> 2.4f
            | Shattered -> 3f
            | Overload -> 4f
            | _ -> 0f

    // HP formulas
    let calcTotalHp (b, c) =
        b.BaseHp * (1f + Entity.getStatPercent PercStat.Hp c) + Entity.getStatFlat FlatStat.Hp c

    // Attack formulas
    let calcTotalAttack (b, c) =
        b.BaseAttack * (1f + Entity.getStatPercent PercStat.Attack c) + Entity.getStatFlat FlatStat.Attack c
        
    // Defense formulas
    let calcTotalDefense (b, c) =
        b.BaseDefense * (1f + Entity.getStatPercent PercStat.Defense c) + Entity.getStatFlat FlatStat.Defense c

    // Elemental mastery formulas
    let calcTotalElementalMastery =
        Entity.getStatFlat FlatStat.ElementalMastery

    // Energy recharge formulas
    let calcTotalEnergyRecharge entity =
        1f + Entity.getStatPercent PercStat.EnergyRecharge entity

    // Crit rate formulas
    let calcTotalCriticalHit =
        Entity.getStatPercent PercStat.CriticalHit

    // Crit damage formulas
    let calcTotalCriticalDamage =
        Entity.getStatPercent PercStat.CriticalDamage

    // Outgoing damage formulas
    let calcAmplifyingBonus em =
        2.78f * (em / (em + 1400f))

    let calcAmplifyingMultiplier reaction amplifyingBonus reactionBonus =
        getAmpifyingReactionMultiplier reaction * (1f + amplifyingBonus + reactionBonus)

    let calcTransformativeBonus em =
        16f * (em / (em + 2000f))

    let calcTransformativeDamageCharacter reaction characterLevel transformativeBonus reactionBonus =
        // TODO: Decide if we're going to use the datamined coefficients or the KQM formula.
        // I like not depending on the datamine being updated, but I think the KQM formula
        // was determined using polynomial regression, which would make it marginally less
        // accurate.
        let levelMult = ElementalCoefficients.getCharacterLevelMultiplier characterLevel
        uint32 (levelMult * getTransformativeReactionMultiplier reaction * (1f + transformativeBonus + reactionBonus))

    let calcTransformativeDamageEnemy reaction enemyLevel transformativeBonus reactionBonus =
        let levelMult = ElementalCoefficients.getEnemyLevelMultiplier enemyLevel
        uint32 (levelMult * getTransformativeReactionMultiplier reaction * (1f + transformativeBonus + reactionBonus))

    let calcAverageCritMultiplier critRate critDamage =
        1f + (min (critRate) 0f) * critDamage

    let calcCritMultiplier critDamage =
        1f + critDamage

    let calcOutgoingDamage (abilityStat: float32) abilityMult bonusFlat bonusMult =
        abilityStat * abilityMult + bonusFlat * bonusMult

    // Incoming damage formulas
    let calcEnemyDefense (level: uint32) =
        5f * float32 level + 500f

    let calcBaseDefenseMultiplier defense (attackerLevel: uint32) =
        1f - (defense / (defense + 5f * float32 attackerLevel + 500f))

    let calcLevelDiffDefenseMultiplier (attackerLevel: uint32) (defenderLevel: uint32) defReductionPerc defIgnoredPerc =
        (float32 attackerLevel + 100f) / (float32 attackerLevel + 100f + (float32 defenderLevel + 100f) * (1f - (min 0.9f defReductionPerc)) * (1f - defIgnoredPerc))

    let calcDefenseMultiplier (defense: float32) (attackerLevel: uint32) (defenderLevel: uint32) defReductionPerc defIgnoredPerc =
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

    let calcIncomingTransformativeDamage baseDamage resMult =
        baseDamage * resMult

    let calcIncomingDamage (amplifyingMult: float32) baseDamage defMult resMult dmgReductionMult =
        amplifyingMult * baseDamage * defMult * resMult * dmgReductionMult