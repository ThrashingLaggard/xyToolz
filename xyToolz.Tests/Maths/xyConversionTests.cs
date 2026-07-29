using System.Reflection;
using xyToolz.Maths;
using Xunit;

namespace xyToolz.Tests.Maths;

/// <summary>
/// xyConversion accumulates results into private static fields ("endergebnis"/"endausgabe")
/// instead of returning them purely, so every test must reset that shared state first —
/// otherwise results leak between test cases and even between calls within the same case
/// (e.g. Bin_to_Hex calls Bin_to_Dec, both touching the same fields). Reset via reflection
/// since no public reset API exists; this is a documented limitation, not something to
/// silently work around in production code as part of this test-writing pass.
/// </summary>
public class xyConversionTests
{
    public xyConversionTests()
    {
        ResetStaticState();
    }

    private static void ResetStaticState()
    {
        var type = typeof(xyConversion);
        type.GetField("endergebnis", BindingFlags.NonPublic | BindingFlags.Static)!.SetValue(null, "");
        type.GetField("endausgabe", BindingFlags.NonPublic | BindingFlags.Static)!.SetValue(null, "");
    }

    [Fact]
    public void HEX_to_DEC_ConvertsSingleByteHexValue()
    {
        // Arrange / Act
        string result = xyConversion.HEX_to_DEC("FF");

        // Assert
        Assert.Equal("255", result);
    }

    [Fact]
    public void Bin_to_Dec_ConvertsSimpleBinaryValue()
    {
        // Arrange / Act
        string result = xyConversion.Bin_to_Dec("1010");

        // Assert
        Assert.Equal("10", result);
    }
}
