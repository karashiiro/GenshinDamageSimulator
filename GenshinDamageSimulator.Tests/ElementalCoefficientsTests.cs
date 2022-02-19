using GenshinDamageSimulator.Data;
using Xunit;

namespace GenshinDamageSimulator.Tests;

public class ElementalCoefficientsTests
{
    [Fact]
    public void DataLoad_Works()
    {
        var value = ElementalCoefficients.getCharacterLevelMultiplier(1);
        Assert.Equal(17.165605545043945f, value);
    }
}