namespace GenshinDamageSimulator.Tests.Data.Characters;

public static class Noelle
{
    public static readonly Entity Noelle90 = Entity.CreateCharacter(
        // TODO: Cross-reference with KQM library
        new BasicEntityParams
        {
            BaseHp = 12071,
            BaseAttack = 191,
            BaseDefense = 559 + 240, // TODO: Where is the +240 coming from?
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
            Weapon = new Weapon(23, null),
            Artifacts = Array.Empty<Artifact>(),
        });
}