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
        // TODO: Get initial HP from entity
        var testNpcState0 = new EntityState(sim.FreeId(), 12071, 0, ElementalAuraState.Create());
        var testNpcState1 = new EntityState(sim.FreeId(), 18566, 0, ElementalAuraState.FromDictionary(new Dictionary<Element, ElementalAuraData>()));
        sim = sim.CombatantAdd(testNpc0, testNpcState0);
        sim = sim.PartyAdd(testNpcState0.Id);
        sim = sim.CombatantAdd(testNpc1, testNpcState1);
        sim = sim.TalentDamage(DamageType.Physical, TalentStat.Defense, 1.5640f, Critical.NoCritical, testNpcState0.Id, testNpcState1.Id);
        var result = sim.LastEventResult.Match(
            _ => 0f,
            _ => 0f,
            _ => 0f,
            _ => 0f,
            _ => 0f,
            _ => 0f,
            damageResult => damageResult.DamageAmount,
            _ => 0f,
            _ => 0f);
        Assert.Equal(154f, result);
    }
}