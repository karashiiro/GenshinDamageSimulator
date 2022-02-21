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
                0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 90),
                new CharacterEntityData(StatModifier.CreatePercent(PercStat.CriticalDamage, 0.2f),
                Element.Electro,
                new Weapon(120, StatModifier.CreatePercent(PercStat.PhysicalBonus, 0.5f)),
                Array.Empty<Artifact>()));
            var testNpcState0 = new EntityState(EntityId.Create(1), 20000, 0, ElementalAuraState.Create());

            var testNpc1 = Entity.CreateEnemy(new BasicEntityData(2000, 500, 200,
                0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 100));
            var testNpcState1 = new EntityState(EntityId.Create(2), 20000, 0, ElementalAuraState.FromDictionary(new Dictionary<Element, ElementalAura>()));

            var sim = SimulationState.Create();
            sim = sim.DoEvent(
                GameEvent.NewCombatantAdd(new CombatantAddEvent(testNpc0, testNpcState0)), EntityId.Create(0), EntityId.Create(0));
            sim = sim.DoEvent(
                GameEvent.NewPartyAdd(new PartyAddEvent()), EntityId.Create(0), EntityId.Create(1));
            sim = sim.DoEvent(
                GameEvent.NewCombatantAdd(new CombatantAddEvent(testNpc1, testNpcState1)), EntityId.Create(0), EntityId.Create(0));
            sim = sim.DoEvent(
                GameEvent.NewTalentDamage(new TalentDamageEvent(DamageType.Physical, BaseStat.Attack, 1f,
                    Critical.NoCritical)), EntityId.Create(1), EntityId.Create(2));
            _ = sim.StepBack();
        }
    }
}