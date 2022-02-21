namespace GenshinDamageSimulator

open EntityLogic
open ElementalAura
open Formulas

exception InvalidEventException of string * GameEvent

module EventHandling =
    let handleDamageEventOutgoing (attacker: Entity) event =
        let attackerBasicData = match attacker with
                                | CharacterEntity (cd, _) -> cd
                                | EnemyEntity ed -> ed
        let attackerBaseStat = getBNpcBaseStat event.DamageStat attacker
        let attackerDamageBonus = getBNpcDamageBonusPercent event.DamageType attacker
        let attackerCriticalHit = getBNpcStatPercent PercStat.CriticalHit attacker
        let attackerCriticalDamage = getBNpcStatPercent PercStat.CriticalDamage attacker
        let outgoingDamage = calcOutgoingDamage attackerBaseStat event.DamageStatMultiplier 0u attackerDamageBonus
        match event.Critical with
        | FullCritical -> uint32 (float32 (outgoingDamage) * calcCritMultiplier attackerCriticalDamage)
        | AverageCritical -> uint32 (float32 (outgoingDamage) * calcAverageCritMultiplier attackerCriticalHit attackerCriticalDamage)
        | NoCritical -> outgoingDamage

    let handleDamageEventIncoming (attacker: Entity) defender outgoingDamage event =
        let attackerBasicData = match attacker with
                                | CharacterEntity (cd, _) -> cd
                                | EnemyEntity ed -> ed
        let defenderBasicData = match defender with
                                | CharacterEntity (cd, _) -> cd
                                | EnemyEntity ed -> ed
        let defenderDefenseBaseStat = match defender with
                                      | CharacterEntity (_, _) -> getBNpcBaseStat BaseStat.Defense defender
                                      | EnemyEntity ed -> calcEnemyDefense ed.Level
        let defenderResistanceBaseStat = getBNpcBaseResStat event.DamageType defender
        let defMult = calcDefenseMultiplier defenderDefenseBaseStat attackerBasicData.Level defenderBasicData.Level 0f 0f
        let resMult = calcResMultiplier defenderResistanceBaseStat (getBNpcDamageResPercent event.DamageType defender) 0f
        calcIncomingDamage 1f outgoingDamage defMult resMult (calcDamageReductionMultiplier 0f)

    let handleDamageEvent event (attacker: Entity, _: EntityState) (defender, defenderState) =
        let eo = getElementForDamageType event.DamageType
        event
        |> handleDamageEventOutgoing attacker
        |> handleDamageEventIncoming attacker defender <| event
        |> fun incomingDamage
            -> { TargetId = defenderState.Id
                 DamageAmount = incomingDamage
                 DamageAura =
                    match eo with
                    | Some element
                        -> Some(wrapAuraData { Element = element
                                               ApplicationSkillId = 0u
                                               ApplicationSkillIcdMs = 0f
                                               GaugeUnits = 0f |> GaugeUnits
                                               Permanent = false })
                    | None -> None }

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
               | Elapse e -> ElapseResult ({ TimeElapsedMs = e.TimeElapsedMs })
               | CombatantAdd e -> CombatantAddResult ({ BNpc = e.BNpc; BNpcState = e.BNpcState })
               | _ -> raise (InvalidEventException("No such parameterless event exists", event))