namespace GenshinDamageSimulator

open Entity
open Formulas

module EventHandling =
    let handleDamageEventOutgoing attacker event =
        let attackerBaseStat = getBNpcBaseStat attacker event.DamageStat
        let attackerDamageBonus = getBNpcDamageBonusPercent attacker event.DamageType
        let attackerCriticalHit = getBNpcStatPercent attacker PercStat.CriticalHit
        let attackerCriticalDamage = getBNpcStatPercent attacker PercStat.CriticalDamage
        let outgoingDamage = calcOutgoingDamage attackerBaseStat event.DamageStatMultiplier 0u attackerDamageBonus
        match event.Critical with
        | Critical.FullCritical -> uint32 (float32 (outgoingDamage) * calcCritMultiplier attackerCriticalDamage)
        | Critical.AverageCritical -> uint32 (float32 (outgoingDamage) * calcAverageCritMultiplier attackerCriticalHit attackerCriticalDamage)
        | Critical.NoCritical -> outgoingDamage

    let handleDamageEventIncoming attacker defender event outgoingDamage =
        let defenderDefenseBaseStat = match defender.NpcType with
                                        | BattleNpcType.Character -> getBNpcBaseStat defender BaseStat.Defense
                                        | BattleNpcType.Enemy -> calcEnemyDefense defender.Level
        let defenderResistanceBaseStat = getBNpcBaseResStat defender event.DamageType
        let defMult = calcDefenseMultiplier defenderDefenseBaseStat attacker.Level defender.Level 0f 0f
        let resMult = calcResMultiplier defenderResistanceBaseStat (getBNpcDamageResPercent defender event.DamageType) 0f
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