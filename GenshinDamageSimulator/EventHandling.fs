namespace GenshinDamageSimulator

open Entity
open Formulas

module EventHandling =
    let handleDamageEventOutgoing attacker event =
        let attackerBaseStat = getBNpcBaseStat event.DamageStat attacker
        let attackerDamageBonus = getBNpcDamageBonusPercent event.DamageType attacker
        let attackerCriticalHit = getBNpcStatPercent PercStat.CriticalHit attacker
        let attackerCriticalDamage = getBNpcStatPercent PercStat.CriticalDamage attacker
        let outgoingDamage = calcOutgoingDamage attackerBaseStat event.DamageStatMultiplier 0u attackerDamageBonus
        match event.Critical with
        | Critical.FullCritical -> uint32 (float32 (outgoingDamage) * calcCritMultiplier attackerCriticalDamage)
        | Critical.AverageCritical -> uint32 (float32 (outgoingDamage) * calcAverageCritMultiplier attackerCriticalHit attackerCriticalDamage)
        | Critical.NoCritical -> outgoingDamage

    let handleDamageEventIncoming attacker defender event outgoingDamage =
        let defenderDefenseBaseStat = match defender.NpcType with
                                      | BattleNpcType.Character -> getBNpcBaseStat BaseStat.Defense defender
                                      | BattleNpcType.Enemy -> calcEnemyDefense defender.Level
        let defenderResistanceBaseStat = getBNpcBaseResStat event.DamageType defender
        let defMult = calcDefenseMultiplier defenderDefenseBaseStat attacker.Level defender.Level 0f 0f
        let resMult = calcResMultiplier defenderResistanceBaseStat (getBNpcDamageResPercent event.DamageType defender) 0f
        calcIncomingDamage 1f outgoingDamage defMult resMult (calcDamageReductionMultiplier 0f)

    let handleDamageEvent event (attacker, _) (defender, _) =
        event
        |> handleDamageEventOutgoing attacker
        |> fun outgoingDamage -> event, outgoingDamage
        ||> handleDamageEventIncoming attacker defender
        |> fun incomingDamage -> { DamageAmount = incomingDamage }

    let handleEvent event attacker defender =
        match event with
        | TalentDamage e -> GameEventResult.DamageResult (handleDamageEvent e attacker defender)
        | TalentHeal _ -> GameEventResult.HealResult ({ HealAmount = 0u })