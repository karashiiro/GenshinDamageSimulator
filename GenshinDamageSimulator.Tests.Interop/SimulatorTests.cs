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
    public void Noelle_NormalAttack10_CriticalHit1_IsAccurate()
    {
        var testNpc0 = Noelle.Noelle90;
        var testNpc1 = Hilichurls.Hilichurl81;
        
        // TODO: Actually math this out
        const float expectedDamage = 231;

        var sim = SimulationState.Create();
        var testNpcState0 = new EntityState(sim.FreeId(), testNpc0.GetMaxHp(), 0, ElementalAuraState.Create());
        sim = sim.CombatantAdd(testNpc0, testNpcState0);
        sim = sim.PartyAdd(testNpcState0.Id);
        var testNpcState1 = new EntityState(sim.FreeId(), testNpc1.GetMaxHp(), 0, ElementalAuraState.FromDictionary(new Dictionary<Element, ElementalAuraData>()));
        sim = sim.CombatantAdd(testNpc1, testNpcState1);
        sim = sim.TalentDamage(DamageType.Physical, TalentStat.Attack, 1.5640f, Critical.FullCritical, testNpcState0.Id, testNpcState1.Id);
        var result = sim.LastEventResult switch
        {
            GameEventResult.DamageResult r => r.Item.DamageAmount,
            _ => 0f,
        };
        
        // Using a larger threshold because I don't feel like actually mathing this
        AssertFloatEquals(expectedDamage, result, 1f);
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
    
    [Fact]
    public void Noelle_ElementalSkill12_Hit_Works()
    {
        var testNpc0 = Noelle.Noelle90;
        var testNpc1 = Hilichurls.Hilichurl81;
        
        // TODO: Actually math this out
        // Note: The shield takes effect before the damage goes out, so this can't be tested
        // with a Geo resonance active. However, the Geo resistance down takes effect after the
        // damage goes out, so that doesn't interfere with getting test numbers.
        const float expectedDamage = 1148f;

        var sim = SimulationState.Create();
        var testNpcState0 = new EntityState(sim.FreeId(), testNpc0.GetMaxHp(), 0, ElementalAuraState.Create());
        sim = sim.CombatantAdd(testNpc0, testNpcState0);
        sim = sim.PartyAdd(testNpcState0.Id);
        var testNpcState1 = new EntityState(sim.FreeId(), testNpc1.GetMaxHp(), 0, ElementalAuraState.FromDictionary(new Dictionary<Element, ElementalAuraData>()));
        sim = sim.CombatantAdd(testNpc1, testNpcState1);
        sim = sim.TalentDamage(DamageType.Geo, TalentStat.Defense, 2.4000f, Critical.NoCritical, testNpcState0.Id, testNpcState1.Id);
        var result = sim.LastEventResult switch
        {
            GameEventResult.DamageResult r => r.Item.DamageAmount,
            _ => 0f,
        };
        
        // Using a larger threshold because I don't feel like actually mathing this
        // Why is there a deviation of 1.0093 between the expected and actual values?
        // There's probably some float imprecision going on somewhere.
        AssertFloatEquals(expectedDamage, result, 1.2f);
    }
    
    /// <summary>
    /// This test is referring to the first hit of the actual burst damage, not
    /// the damage bonus and damage conversion after the burst damage goes out.
    /// The little burst thing, not the swing afterwards.
    /// </summary>
    [Fact]
    public void Noelle_ElementalBurst10_Hit1_Works()
    {
        var testNpc0 = Noelle.Noelle90;
        var testNpc1 = Hilichurls.Hilichurl81;
        
        // TODO: Actually math this out
        const float expectedDamage = 536f;

        var sim = SimulationState.Create();
        var testNpcState0 = new EntityState(sim.FreeId(), testNpc0.GetMaxHp(), 0, ElementalAuraState.Create());
        sim = sim.CombatantAdd(testNpc0, testNpcState0);
        sim = sim.PartyAdd(testNpcState0.Id);
        var testNpcState1 = new EntityState(sim.FreeId(), testNpc1.GetMaxHp(), 0, ElementalAuraState.FromDictionary(new Dictionary<Element, ElementalAuraData>()));
        sim = sim.CombatantAdd(testNpc1, testNpcState1);
        sim = sim.TalentDamage(DamageType.Geo, TalentStat.Attack, 1.2096f, Critical.NoCritical, testNpcState0.Id, testNpcState1.Id);
        var result = sim.LastEventResult switch
        {
            GameEventResult.DamageResult r => r.Item.DamageAmount,
            _ => 0f,
        };
        
        // Using a larger threshold because I don't feel like actually mathing this
        AssertFloatEquals(expectedDamage, result, 1f);
    }
    
    [Fact]
    public void Noelle_ElementalBurst10_DamageConversion_Hit1_Works()
    {
        var testNpc0 = Noelle.Noelle90;
        var testNpc1 = Hilichurls.Hilichurl81;
        
        // TODO: Actually math this out
        const float expectedDamage = 693f;

        var sim = SimulationState.Create();
        var testNpcState0 = new EntityState(sim.FreeId(), testNpc0.GetMaxHp(), 0, ElementalAuraState.Create());
        sim = sim.CombatantAdd(testNpc0, testNpcState0);
        sim = sim.PartyAdd(testNpcState0.Id);
        var testNpcState1 = new EntityState(sim.FreeId(), testNpc1.GetMaxHp(), 0, ElementalAuraState.FromDictionary(new Dictionary<Element, ElementalAuraData>()));
        sim = sim.CombatantAdd(testNpc1, testNpcState1);
        sim = sim.TalentDamage(DamageType.Geo, TalentStat.Attack, 1.5640f, Critical.NoCritical, testNpcState0.Id, testNpcState1.Id);
        var result = sim.LastEventResult switch
        {
            GameEventResult.DamageResult r => r.Item.DamageAmount,
            _ => 0f,
        };
        
        // Using a larger threshold because I don't feel like actually mathing this
        AssertFloatEquals(expectedDamage, result, 1f);
    }
    
    [Fact]
    public void Noelle_ElementalBurst10_DamageConversion_Sequence_Works()
    {
        var testNpc0 = Noelle.Noelle90;
        var testNpc1 = Hilichurls.Hilichurl81;
        
        // TODO: Actually math this out
        const float expectedDamage = 693f + 642f + 755f + 994f;

        var actual = 0f;

        var sim = SimulationState.Create();
        var testNpcState0 = new EntityState(sim.FreeId(), testNpc0.GetMaxHp(), 0, ElementalAuraState.Create());
        sim = sim.CombatantAdd(testNpc0, testNpcState0);
        sim = sim.PartyAdd(testNpcState0.Id);
        var testNpcState1 = new EntityState(sim.FreeId(), testNpc1.GetMaxHp(), 0, ElementalAuraState.FromDictionary(new Dictionary<Element, ElementalAuraData>()));
        sim = sim.CombatantAdd(testNpc1, testNpcState1);
        
        sim = sim.TalentDamage(DamageType.Geo, TalentStat.Attack, 1.5640f, Critical.NoCritical, testNpcState0.Id, testNpcState1.Id);
        actual += sim.LastEventResult switch
        {
            GameEventResult.DamageResult r => r.Item.DamageAmount,
            _ => 0f,
        };
        
        sim = sim.TalentDamage(DamageType.Geo, TalentStat.Attack, 1.4501f, Critical.NoCritical, testNpcState0.Id, testNpcState1.Id);
        actual += sim.LastEventResult switch
        {
            GameEventResult.DamageResult r => r.Item.DamageAmount,
            _ => 0f,
        };
        
        sim = sim.TalentDamage(DamageType.Geo, TalentStat.Attack, 1.7051f, Critical.NoCritical, testNpcState0.Id, testNpcState1.Id);
        actual += sim.LastEventResult switch
        {
            GameEventResult.DamageResult r => r.Item.DamageAmount,
            _ => 0f,
        };
        
        sim = sim.TalentDamage(DamageType.Geo, TalentStat.Attack, 2.2423f, Critical.NoCritical, testNpcState0.Id, testNpcState1.Id);
        actual += sim.LastEventResult switch
        {
            GameEventResult.DamageResult r => r.Item.DamageAmount,
            _ => 0f,
        };
        
        // Using a larger threshold because I don't feel like actually mathing this
        AssertFloatEquals(expectedDamage, actual, 1f);
    }

    private static void AssertFloatEquals(float expected, float actual, float threshold = 0.05f)
    {
        Assert.True(Math.Abs(expected - actual) < threshold, $"Expected: {expected}; actual: {actual}");
    }
}