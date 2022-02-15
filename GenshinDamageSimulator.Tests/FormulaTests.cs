using System;
using Xunit;

namespace GenshinDamageSimulator.Tests
{
    public class FormulaTests
    {
        [Fact]
        public void SingleAbility_Works()
        {
            var testNpc = new BattleNpc(2000, 500, 200,
                StatModifier.NewPercStatModifier(new PercStatModifier(PercStatType.CriticalDamage, 0.2f)),
                Element.Electro,
                new Weapon(120, StatModifier.NewPercStatModifier(new PercStatModifier(PercStatType.Physical, 0.5f))),
                Array.Empty<Artifact>());
            var testNpcState = new BattleNpcState();

            var sim = SimulationState.Create();
            _ = sim.AddCombatant(testNpc, testNpcState);
        }
    }
}