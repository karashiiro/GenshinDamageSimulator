using System;
using System.Collections.Generic;
using Xunit;

namespace GenshinDamageSimulator.Tests.Interop;

public class FormulaTests
{
    [Fact]
    public void NormalAttack_Works()
    {
        var testNpc0 = Entity.CreateCharacter(
            new BasicEntityParams
            {
                BaseHp = 2000,
                BaseAttack = 500,
                BaseDefense = 200,
                BasePhysicalRes = 0f,
                BasePyroRes = 0f,
                BaseHydroRes = 0f,
                BaseElectroRes = 0f,
                BaseCryoRes = 0f,
                BaseAnemoRes = 0f,
                BaseGeoRes = 0f,
                BaseDendroRes = 0f,
                Level = 90,
            },
            new CharacterEntityParams
            {
                MainStat = StatModifier.CreatePercent(PercStat.CriticalDamage, 0.2f),
                Element = Element.Geo,
                Weapon = new Weapon(120, StatModifier.CreatePercent(PercStat.PhysicalBonus, 0.5f)),
                Artifacts = Array.Empty<Artifact>(),
            });

        var testNpc1 = Entity.CreateEnemy(new BasicEntityParams
        {
            BaseHp = 2000,
            BaseAttack = 500,
            BaseDefense = 200,
            BasePhysicalRes = 0f,
            BasePyroRes = 0f,
            BaseHydroRes = 0f,
            BaseElectroRes = 0f,
            BaseCryoRes = 0f,
            BaseAnemoRes = 0f,
            BaseGeoRes = 0f,
            BaseDendroRes = 0f,
            Level = 100,
        });

        var sim = SimulationState.Create();
        var testNpcState0 = new EntityState(sim.FreeId(), 20000, 0, ElementalAuraState.Create());
        var testNpcState1 = new EntityState(sim.FreeId(), 20000, 0, ElementalAuraState.FromDictionary(new Dictionary<Element, ElementalAuraData>()));
        sim = sim.CombatantAdd(testNpc0, testNpcState0);
        sim = sim.PartyAdd(testNpcState0.Id);
        sim = sim.CombatantAdd(testNpc1, testNpcState1);
        sim = sim.TalentDamage(DamageType.Physical, TalentStat.Attack, 1f, Critical.NoCritical, testNpcState0.Id, testNpcState1.Id);
        _ = sim.StepBack();
    }
}