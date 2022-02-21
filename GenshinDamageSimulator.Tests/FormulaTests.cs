using System;
using System.Linq;
using Microsoft.FSharp.Collections;
using Xunit;

namespace GenshinDamageSimulator.Tests
{
    public class FormulaTests
    {
        [Fact]
        public void SingleAbility_Works()
        {
            var testNpc0 = new Entity.CharacterEntity(new Tuple<BasicEntityData, CharacterEntityData>(new BasicEntityData(2000, 500, 200,
                0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f,
                90), new CharacterEntityData(StatModifier.NewPercStatModifier(new PercStatModifier(PercStat.CriticalDamage, 0.2f)),
                Element.Electro,
                new Weapon(120, StatModifier.NewPercStatModifier(new PercStatModifier(PercStat.PhysicalBonus, 0.5f))),
                Array.Empty<Artifact>())));
            var testNpcState0 = new EntityState(1, 20000, 0, new FSharpMap<Element, Aura>(Enumerable.Empty<Tuple<Element, Aura>>()));

            var testNpc1 = new BasicEntityData(2000, 500, 200,
                0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f,
                100,
                StatModifier.NewPercStatModifier(new PercStatModifier(PercStat.CriticalDamage, 0.2f)),
                Element.Electro,
                new Weapon(120, StatModifier.NewPercStatModifier(new PercStatModifier(PercStat.PhysicalBonus, 0.5f))),
                Array.Empty<Artifact>());
            var testNpcState1 = new EntityState(2, 20000, 0, new FSharpMap<Element, Aura>(Enumerable.Empty<Tuple<Element, Aura>>()));

            var sim = SimulationState.Create();
            sim = sim.DoEvent(
                GameEvent.NewCombatantAdd(new CombatantAddEvent(testNpc0, testNpcState0)), 0, 0);
            sim = sim.DoEvent(
                GameEvent.NewPartyAdd(new PartyAddEvent()), 0, 1);
            sim = sim.DoEvent(
                GameEvent.NewCombatantAdd(new CombatantAddEvent(testNpc1, testNpcState1)), 0, 0);
            sim = sim.DoEvent(
                GameEvent.NewTalentDamage(new TalentDamageEvent(DamageType.Physical, BaseStat.Attack, 1f,
                    Critical.NoCritical)), 1, 2);
            _ = sim.StepBack();
        }
    }
}