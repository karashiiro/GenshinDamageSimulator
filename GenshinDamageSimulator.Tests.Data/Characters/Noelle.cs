namespace GenshinDamageSimulator.Tests.Data.Characters;

public static class Noelle
{
    public static readonly Entity Noelle90 = Entity.CreateCharacter(
        new BasicEntityParams
        {
            // https://library.keqingmains.com/characters/geo/noelle
            // https://genshin-impact.fandom.com/wiki/Noelle#Ascensions_and_Stats
            BaseHp = 12071,
            BaseAttack = 191,
            BaseDefense = 799,
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
            MainStat = StatModifier.CreatePercent(PercStat.Defense, 0.3f),
            CriticalHit = StatModifier.CreatePercent(PercStat.CriticalHit, 0.05f),
            CriticalDamage = StatModifier.CreatePercent(PercStat.CriticalDamage, 0.5f),
            EnergyRecharge = StatModifier.CreatePercent(PercStat.EnergyRecharge, 1f),
            Element = Element.Geo,
            Weapon = new Weapon(23, null), // Waster Greatsword
            Artifacts = Array.Empty<Artifact>(),
        });
}