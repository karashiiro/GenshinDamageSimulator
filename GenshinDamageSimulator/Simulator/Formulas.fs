namespace GenshinDamageSimulator

open GenshinDamageSimulator.Data

// https://genshin-impact.fandom.com/wiki/Damage
// https://library.keqingmains.com/combat-mechanics/damage/damage-formula

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

    let calcTotalHp entity =
        match entity with
        | CharacterEntity (b, _) -> 
            b.BaseHp * (1f + Entity.getStatPercent PercStat.Hp entity) + Entity.getStatFlat FlatStat.Hp entity
        | EnemyEntity b -> b.BaseHp * (1f + Entity.getStatPercent PercStat.Hp entity) + Entity.getStatFlat FlatStat.Hp entity

    let calcTotalAttack entity =
        match entity with
        | CharacterEntity (b, c) -> 
            (b.BaseAttack + c.Weapon.Attack) * (1f + Entity.getStatPercent PercStat.Attack entity) + Entity.getStatFlat FlatStat.Attack entity
        | EnemyEntity b -> b.BaseAttack * (1f + Entity.getStatPercent PercStat.Attack entity) + Entity.getStatFlat FlatStat.Attack entity
        
    let calcTotalDefense entity =
        match entity with
        | CharacterEntity (b, _) -> 
            b.BaseDefense * (1f + Entity.getStatPercent PercStat.Defense entity) + Entity.getStatFlat FlatStat.Defense entity
        | EnemyEntity b -> b.BaseDefense * (1f + Entity.getStatPercent PercStat.Defense entity) + Entity.getStatFlat FlatStat.Defense entity

    let calcTotalElementalMastery =
        Entity.getStatFlat FlatStat.ElementalMastery

    let calcTotalEnergyRecharge entity =
        1f + Entity.getStatPercent PercStat.EnergyRecharge entity

    let calcTotalCriticalHit =
        Entity.getStatPercent PercStat.CriticalHit

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
    let calcDefenseMultiplierRaw attackerLevel defenderDefense defReductionPerc defIgnoredPerc =
        // https://genshin-impact.fandom.com/wiki/Defense
        (1f - defenderDefense / (defenderDefense + 5f * float32 attackerLevel + 500f)) * (1f - (min 0.9f defReductionPerc)) * (1f - defIgnoredPerc)

    let calcDefenseMultiplier attacker defender defReductionPerc defIgnoredPerc =
        let attackerLevel, defenderDefense =
            match (attacker, defender) with
            | CharacterEntity (bc, _), EnemyEntity be -> bc.Level, be.BaseDefense
            | EnemyEntity be, CharacterEntity (bc, _) -> bc.Level, be.BaseDefense
            | CharacterEntity (bc1, _), CharacterEntity (bc2, _) -> bc1.Level, bc2.BaseDefense
            | EnemyEntity be1, EnemyEntity be2 -> be1.Level, be2.BaseDefense
        calcDefenseMultiplierRaw attackerLevel defenderDefense defReductionPerc defIgnoredPerc

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