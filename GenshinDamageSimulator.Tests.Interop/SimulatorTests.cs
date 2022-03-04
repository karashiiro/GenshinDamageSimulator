using GenshinDamageSimulator.Tests.Data.Characters;
using GenshinDamageSimulator.Tests.Data.Enemies;
using System.Collections.Generic;
using Xunit;

namespace GenshinDamageSimulator.Tests.Interop;

public class SimulatorTests
{
    [Fact]
    public void NormalAttack_Works()
    {
        var testNpc0 = Noelle.Noelle90;
        var testNpc1 = Hilichurls.Hilichurl81;

        var sim = SimulationState.Create();
        var testNpcState0 = new EntityState(sim.FreeId(), testNpc0.GetMaxHp(), 0, ElementalAuraState.Create());
        sim = sim.CombatantAdd(testNpc0, testNpcState0);
        sim = sim.PartyAdd(testNpcState0.Id);
        var testNpcState1 = new EntityState(sim.FreeId(), testNpc1.GetMaxHp(), 0, ElementalAuraState.FromDictionary(new Dictionary<Element, ElementalAuraData>()));
        sim = sim.CombatantAdd(testNpc1, testNpcState1);
        sim = sim.TalentDamage(DamageType.Physical, TalentStat.Attack, 1.5640f, Critical.NoCritical, testNpcState0.Id, testNpcState1.Id);
        var result = sim.LastEventResult switch
        {
            GameEventResult.DamageResult r => r.Item.DamageAmount,
            _ => 0f,
        };
        Assert.Equal(154f, result);
    }
}