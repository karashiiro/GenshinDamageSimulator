namespace GenshinDamageSimulator

exception InvalidEventException of string * GameEvent

module EventHandling =
    let handleDamageEventOutgoing (attacker: Entity) event =
        let attackerBasicData = match attacker with
                                | CharacterEntity (cd, _) -> cd
                                | EnemyEntity ed -> ed
        let attackerBaseStat = Entity.getBaseStat event.DamageStat attacker
        let attackerDamageBonus = Entity.getDamageBonusPercent event.DamageType attacker
        let attackerCriticalHit = Entity.getStatPercent PercStat.CriticalHit attacker
        let attackerCriticalDamage = Entity.getStatPercent PercStat.CriticalDamage attacker
        let outgoingDamage = Formulas.calcOutgoingDamage attackerBaseStat event.DamageStatMultiplier 0u attackerDamageBonus
        match event.Critical with
        | FullCritical -> uint32 (float32 (outgoingDamage) * Formulas.calcCritMultiplier attackerCriticalDamage)
        | AverageCritical -> uint32 (float32 (outgoingDamage) * Formulas.calcAverageCritMultiplier attackerCriticalHit attackerCriticalDamage)
        | NoCritical -> outgoingDamage

    let handleDamageEventIncoming (attacker: Entity) defender outgoingDamage event =
        let attackerBasicData = match attacker with
                                | CharacterEntity (cd, _) -> cd
                                | EnemyEntity ed -> ed
        let defenderBasicData = match defender with
                                | CharacterEntity (cd, _) -> cd
                                | EnemyEntity ed -> ed
        let defenderDefenseBaseStat = match defender with
                                      | CharacterEntity (_, _) -> Entity.getBaseStat BaseStat.Defense defender
                                      | EnemyEntity ed -> Formulas.calcEnemyDefense ed.Level
        let defenderResistanceBaseStat = Entity.getBaseResStat event.DamageType defender
        let defMult = Formulas.calcDefenseMultiplier defenderDefenseBaseStat attackerBasicData.Level defenderBasicData.Level 0f 0f
        let resMult = Formulas.calcResMultiplier defenderResistanceBaseStat (Entity.getDamageResPercent event.DamageType defender) 0f
        Formulas.calcIncomingDamage 1f outgoingDamage defMult resMult (Formulas.calcDamageReductionMultiplier 0f)

    let handleDamageEvent event (attacker: Entity, _: EntityState) (defender, defenderState) =
        let eo = Entity.getElementForDamageType event.DamageType
        event
        |> handleDamageEventOutgoing attacker
        |> handleDamageEventIncoming attacker defender <| event
        |> fun incomingDamage
            -> { TargetId = defenderState.Id
                 DamageAmount = incomingDamage
                 DamageAura =
                    match eo with
                    | Some element
                        -> Some({ Element = element
                                  ApplicationSkillId = 0u
                                  ApplicationSkillIcdMs = 0f
                                  GaugeUnits = 0f |> GaugeUnits
                                  Permanent = false } |> ElementalAura.wrap)
                    | None -> None }

    let handleEvent event sourceOption targetOption =
        match sourceOption, targetOption with
        | Some (source, sourceState), Some (target, targetState)
            -> match event with
               | TalentDamage e -> DamageResult (handleDamageEvent e (source, sourceState) (target, targetState))
               | TalentHeal _ -> HealResult ({ TargetId = targetState.Id; HealAmount = 0u })
               | _ -> raise (InvalidEventException("No such source-target event exists.", event))
        | _, Some (_, targetState)
            -> match event with
               | CombatantRemove _ -> CombatantRemoveResult ({ TargetId = targetState.Id })
               | PartyAdd _ -> PartyAddResult ({ TargetId = targetState.Id })
               | PartyRemove _ -> PartyRemoveResult ({ TargetId = targetState.Id })
               | _ -> raise (InvalidEventException("No such target event exists.", event))
        | _ -> match event with
               | Elapse e -> ElapseResult ({ TimeElapsedMs = e.TimeElapsedMs })
               | CombatantAdd e -> CombatantAddResult ({ Entity = e.Entity; EntityState = e.EntityState })
               | _ -> raise (InvalidEventException("No such parameterless event exists.", event))