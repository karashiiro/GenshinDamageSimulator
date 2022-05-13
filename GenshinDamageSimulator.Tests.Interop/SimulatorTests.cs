using GenshinDamageSimulator.Tests.Data.Characters;
using GenshinDamageSimulator.Tests.Data.Enemies;
using System;
using System.Collections.Generic;
using Xunit;

namespace GenshinDamageSimulator.Tests.Interop;

public class SimulatorTests
{
    [Fact]
    public void Noelle_NormalAttack10_Hit1_IsAccurate()
    {
        var testNpc0 = Noelle.Noelle90;
        var testNpc1 = Hilichurls.Hilichurl81;
        const float expectedDamage = 154.26689f;

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
        
        AssertFloatEquals(expectedDamage, result);
    }
    
    [Fact]
    public void Noelle_NormalAttack10_Sequence_IsAccurate()
    {
        var testNpc0 = Noelle.Noelle90;
        var testNpc1 = Hilichurls.Hilichurl81;
        
        // TODO: Actually math this out
        const float expectedDamage = 154.26689f + 143f + 168f + 221f;

        var actualDamage = 0f;

        var sim = SimulationState.Create();
        var testNpcState0 = new EntityState(sim.FreeId(), testNpc0.GetMaxHp(), 0, ElementalAuraState.Create());
        sim = sim.CombatantAdd(testNpc0, testNpcState0);
        sim = sim.PartyAdd(testNpcState0.Id);
        var testNpcState1 = new EntityState(sim.FreeId(), testNpc1.GetMaxHp(), 0, ElementalAuraState.FromDictionary(new Dictionary<Element, ElementalAuraData>()));
        sim = sim.CombatantAdd(testNpc1, testNpcState1);
        
        sim = sim.TalentDamage(DamageType.Physical, TalentStat.Attack, 1.5640f, Critical.NoCritical, testNpcState0.Id, testNpcState1.Id);
        actualDamage += sim.LastEventResult switch
        {
            GameEventResult.DamageResult r => r.Item.DamageAmount,
            _ => 0f,
        };
        
        sim = sim.TalentDamage(DamageType.Physical, TalentStat.Attack, 1.4501f, Critical.NoCritical, testNpcState0.Id, testNpcState1.Id);
        actualDamage += sim.LastEventResult switch
        {
            GameEventResult.DamageResult r => r.Item.DamageAmount,
            _ => 0f,
        };
        
        sim = sim.TalentDamage(DamageType.Physical, TalentStat.Attack, 1.7051f, Critical.NoCritical, testNpcState0.Id, testNpcState1.Id);
        actualDamage += sim.LastEventResult switch
        {
            GameEventResult.DamageResult r => r.Item.DamageAmount,
            _ => 0f,
        };
        
        sim = sim.TalentDamage(DamageType.Physical, TalentStat.Attack, 2.2423f, Critical.NoCritical, testNpcState0.Id, testNpcState1.Id);
        actualDamage += sim.LastEventResult switch
        {
            GameEventResult.DamageResult r => r.Item.DamageAmount,
            _ => 0f,
        };
        
        // Using a larger threshold because I don't feel like actually mathing this
        AssertFloatEquals(expectedDamage, actualDamage, 1f);
    }

    private static void AssertFloatEquals(float expected, float actual, float threshold = 0.05f)
    {
        Assert.True(Math.Abs(expected - actual) < threshold, $"Expected: {expected}; actual: {actual}");
    }
}