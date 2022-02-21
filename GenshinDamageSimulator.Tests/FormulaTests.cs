using System;
using System.Collections.Generic;
using Xunit;

namespace GenshinDamageSimulator.Tests
{
    public class FormulaTests
    {
        [Fact]
        public void SingleAbility_Works()
        {
            var testNpc0 = Entity.CreateCharacter(new BasicEntityData(2000, 500, 200,
                0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f,
                90), new CharacterEntityData(StatModifier.NewPercStatModifier(new PercStatModifier(PercStat.CriticalDamage, 0.2f)),
                Element.Electro,
                new Weapon(120, StatModifier.NewPercStatModifier(new PercStatModifier(PercStat.PhysicalBonus, 0.5f))),
                Array.Empty<Artifact>()));
            var testNpcState0 = new EntityState(EntityId.NewEntityId(1), 20000, 0, ElementalAuraState.Create());

            var testNpc1 = Entity.NewCharacterEntity(new Tuple<BasicEntityData, CharacterEntityData>(new BasicEntityData(2000, 500, 200,
                0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f,
                100), new CharacterEntityData(StatModifier.NewPercStatModifier(new PercStatModifier(PercStat.CriticalDamage, 0.2f)),
                Element.Electro,
                new Weapon(120, StatModifier.NewPercStatModifier(new PercStatModifier(PercStat.PhysicalBonus, 0.5f))),
                Array.Empty<Artifact>())));
            var testNpcState1 = new EntityState(EntityId.NewEntityId(2), 20000, 0, ElementalAuraState.FromDictionary(new Dictionary<Element, ElementalAura>()));

            var sim = SimulationState.Create();
            sim = sim.DoEvent(
                GameEvent.NewCombatantAdd(new CombatantAddEvent(testNpc0, testNpcState0)), EntityId.NewEntityId(0), EntityId.NewEntityId(0));
            sim = sim.DoEvent(
                GameEvent.NewPartyAdd(new PartyAddEvent()), EntityId.NewEntityId(0), EntityId.NewEntityId(1));
            sim = sim.DoEvent(
                GameEvent.NewCombatantAdd(new CombatantAddEvent(testNpc1, testNpcState1)), EntityId.NewEntityId(0), EntityId.NewEntityId(0));
            sim = sim.DoEvent(
                GameEvent.NewTalentDamage(new TalentDamageEvent(DamageType.Physical, BaseStat.Attack, 1f,
                    Critical.NoCritical)), EntityId.NewEntityId(1), EntityId.NewEntityId(2));
            _ = sim.StepBack();
        }
    }
}