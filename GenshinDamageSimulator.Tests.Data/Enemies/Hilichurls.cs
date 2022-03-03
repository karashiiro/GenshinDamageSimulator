namespace GenshinDamageSimulator.Tests.Data.Enemies;

public static class Hilichurls
{
    public static readonly Entity Hilichurl81 = Entity.CreateEnemy(
        new BasicEntityParams
        {
            // https://genshin-impact.fandom.com/wiki/Level_Scaling/Enemy
            // https://raw.githubusercontent.com/Dimbreath/GenshinData/master/ExcelBinOutput/MonsterExcelConfigData.json
            // https://github.com/Dimbreath/GenshinData/blob/master/ExcelBinOutput/MonsterCurveExcelConfigData.json
            BaseHp = 18947.872f, // TODO: Write function to calculate these precisely
            BaseAttack = 2894.934f,
            BaseDefense = Entity.CalcEnemyBaseDefense(81),
            // https://library.keqingmains.com/enemy-data/hilichurls/hilichurls/hilichurl#resistance-table
            BasePhysicalRes = 0.1f,
            BasePyroRes = 0.1f,
            BaseHydroRes = 0.1f,
            BaseElectroRes = 0.1f,
            BaseCryoRes = 0.1f,
            BaseAnemoRes = 0.1f,
            BaseGeoRes = 0.1f,
            BaseDendroRes = 0.1f,
            Level = 81,
        });
}