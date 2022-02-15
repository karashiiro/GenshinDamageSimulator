using Xunit;

namespace GenshinDamageSimulator.Tests;

public class ElementalCoefficientsTests
{
    [Fact]
    public void DataLoad_Works()
    {
        var playerData = ElementalCoefficients.characterLevelMultipliers;
        Assert.Equal(17.165605545043945f, playerData[1]);
    }
}