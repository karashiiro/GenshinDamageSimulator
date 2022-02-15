using System;
using Xunit;
using static GenshinDamageSimulator.EntityTypes;
using static GenshinDamageSimulator.StatTypes;

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

            var sim = Simulator.CreateSimulation();

            Simulator.AddCombatant(sim, testNpc, testNpcState);
        }
    }
}