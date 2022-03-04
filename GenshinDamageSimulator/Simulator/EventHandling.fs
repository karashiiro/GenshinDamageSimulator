namespace GenshinDamageSimulator

module EventHandling =
    let handleDamageEventOutgoing attacker event =
        attacker
        |>
            match event.DamageStat with
            | TalentStat.Attack -> Formulas.calcTotalAttack
            | TalentStat.Defense -> Formulas.calcTotalDefense
            | TalentStat.Hp -> Formulas.calcTotalHp
        |> fun s -> s, Entity.getDamageBonusPercent event.DamageType attacker
        |> fun (s, b) -> s, b, Entity.getStatPercent PercStat.CriticalHit attacker
        |> fun (s, b, cr) -> s, b, cr, Entity.getStatPercent PercStat.CriticalDamage attacker
        |> fun (s, b, cr, cd) -> cr, cd, Formulas.calcOutgoingDamage s event.DamageStatMultiplier 0f b
        |> fun (cr, cd, o) ->
            match event.Critical with
            | FullCritical -> o * Formulas.calcCritMultiplier cd
            | AverageCritical -> o * Formulas.calcAverageCritMultiplier cr cd
            | NoCritical -> o
        
    let handleDamageEventIncoming attacker defender outgoingDamage event =
        defender
        |> Entity.getBaseResStat event.DamageType
        |> fun s -> s, Formulas.calcDefenseMultiplier attacker defender 0f 0f
        |> fun (s, dm) -> dm, Formulas.calcResMultiplier s (Entity.getDamageResPercent event.DamageType defender) 0f
        |> fun (dm, rm) -> Formulas.calcIncomingDamage 1f outgoingDamage dm rm (Formulas.calcDamageReductionMultiplier 0f)

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
                                  ElementalMastery = Formulas.calcTotalElementalMastery attacker
                                  Gauge = 0f |> Gauge.ofUnits
                                  Permanent = false } |> ElementalAura.wrap)
                    | None -> None }

    let handleEvent event sourceOption targetOption =
        match sourceOption, targetOption with
        | Some (source, sourceState), Some (target, targetState)
            -> match event with
               | TalentDamage e -> Ok (DamageResult (handleDamageEvent e (source, sourceState) (target, targetState)))
               | TalentHeal _ -> Ok (HealResult ({ TargetId = targetState.Id; HealAmount = 0f }))
               | _ -> Error ("No such source-target event exists.", event)
        | _, Some (_, targetState)
            -> match event with
               | CombatantRemove _ -> Ok (CombatantRemoveResult ({ TargetId = targetState.Id }))
               | PartyAdd _ -> Ok (PartyAddResult ({ TargetId = targetState.Id }))
               | PartyRemove _ -> Ok (PartyRemoveResult ({ TargetId = targetState.Id }))
               | _ -> Error ("No such target event exists.", event)
        | _ -> match event with
               | Elapse e -> Ok (ElapseResult ({ TimeElapsedMs = e.TimeElapsedMs }))
               | CombatantAdd e -> Ok (CombatantAddResult ({ Entity = e.Entity; EntityState = e.EntityState }))
               | _ -> Error ("No such parameterless event exists.", event)