﻿namespace GenshinDamageSimulator

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
        | FullCritical -> uint32 (float32 (outgoingDamage) * calcCritMultiplier attackerCriticalDamage)
        | AverageCritical -> uint32 (float32 (outgoingDamage) * calcAverageCritMultiplier attackerCriticalHit attackerCriticalDamage)
        | NoCritical -> outgoingDamage

    let handleDamageEventIncoming attacker defender event outgoingDamage =
        let defenderDefenseBaseStat = match defender.NpcType with
                                      | Character -> getBNpcBaseStat BaseStat.Defense defender
                                      | Enemy -> calcEnemyDefense defender.Level
        let defenderResistanceBaseStat = getBNpcBaseResStat event.DamageType defender
        let defMult = calcDefenseMultiplier defenderDefenseBaseStat attacker.Level defender.Level 0f 0f
        let resMult = calcResMultiplier defenderResistanceBaseStat (getBNpcDamageResPercent event.DamageType defender) 0f
        calcIncomingDamage 1f outgoingDamage defMult resMult (calcDamageReductionMultiplier 0f)

    let handleDamageEvent event (attacker, _) (defender, defenderState) =
        event
        |> handleDamageEventOutgoing attacker
        |> fun outgoingDamage -> event, outgoingDamage
        ||> handleDamageEventIncoming attacker defender
        |> fun incomingDamage -> { TargetId = defenderState.Id; DamageAmount = incomingDamage }

    let handleEvent event sourceOption targetOption =
        match event, sourceOption, targetOption with
        | (Elapse e, _, _)
            -> Some (ElapseResult ({ TimeElapsed = e.TimeElapsed }))
        | (CombatantAdd e, _, Some (_, targetState))
            -> Some (CombatantAddResult ({ TargetId = targetState.Id }))
        | (TalentDamage e, Some (source, sourceState), Some (target, targetState))
            -> Some (DamageResult (handleDamageEvent e (source, sourceState) (target, targetState)))
        | (TalentHeal _, _, Some (target, targetState))
            -> Some (HealResult ({ TargetId = targetState.Id; HealAmount = 0u }))
        | _ -> None