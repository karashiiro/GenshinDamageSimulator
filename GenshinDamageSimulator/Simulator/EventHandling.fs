namespace GenshinDamageSimulator

open Entity
open Formulas

exception InvalidEventException of string * GameEvent

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
        match sourceOption, targetOption with
        | Some (source, sourceState), Some (target, targetState)
            -> match event with
               | TalentDamage e -> DamageResult (handleDamageEvent e (source, sourceState) (target, targetState))
               | _ -> raise (InvalidEventException("No such source-target event exists", event))
        | _, Some (_, targetState)
            -> match event with
               | CombatantRemove _ -> CombatantRemoveResult ({ TargetId = targetState.Id })
               | PartyAdd _ -> PartyAddResult ({ TargetId = targetState.Id })
               | PartyRemove _ -> PartyRemoveResult ({ TargetId = targetState.Id })
               | TalentHeal _ -> HealResult ({ TargetId = targetState.Id; HealAmount = 0u })
               | _ -> raise (InvalidEventException("No such target event exists", event))
        | _ -> match event with
               | Elapse e -> ElapseResult ({ TimeElapsed = e.TimeElapsed })
               | CombatantAdd e -> CombatantAddResult ({ BNpc = e.BNpc })
               | _ -> raise (InvalidEventException("No such parameterless event exists", event))