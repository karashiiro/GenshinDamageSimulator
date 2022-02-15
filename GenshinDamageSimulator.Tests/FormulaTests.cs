using System;
using Xunit;

namespace GenshinDamageSimulator.Tests
{
    public class FormulaTests
    {
        [Fact]
        public void SingleAbility_Works()
        {
            var testNpc0 = new BattleNpc(2000, 500, 200,
                0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f,
                90, BattleNpcType.Character,
                StatModifier.NewPercStatModifier(new PercStatModifier(PercStat.CriticalDamage, 0.2f)),
                Element.Electro,
                new Weapon(120, StatModifier.NewPercStatModifier(new PercStatModifier(PercStat.Physical, 0.5f))),
                Array.Empty<Artifact>());
            var testNpcState0 = new BattleNpcState(20000, 0, Array.Empty<ElementalAura>());

            var testNpc1 = new BattleNpc(2000, 500, 200,
                0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f,
                100, BattleNpcType.Enemy,
                StatModifier.NewPercStatModifier(new PercStatModifier(PercStat.CriticalDamage, 0.2f)),
                Element.Electro,
                new Weapon(120, StatModifier.NewPercStatModifier(new PercStatModifier(PercStat.Physical, 0.5f))),
                Array.Empty<Artifact>());
            var testNpcState1 = new BattleNpcState(20000, 0, Array.Empty<ElementalAura>());

            var sim = SimulationState.Create();
            sim = sim.AddCombatant(0, testNpc0, testNpcState0);
            sim = sim.AddCombatant(1, testNpc1, testNpcState1);
            _ = sim.DoEvent(
                GameEvent.NewTalentDamage(new TalentDamageEvent(DamageType.Physical, BaseStat.Attack, 1f,
                    Critical.NoCritical)), 0, 1);
        }
    }
}