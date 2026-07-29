using xyToolz.Maths;
using Xunit;

namespace xyToolz.Tests.Maths;

public class SumOfDigitsTests
{
    [Theory]
    [InlineData(123, 10, 6)]
    [InlineData(0, 10, 0)]
    [InlineData(-123, 10, 6)]
    [InlineData(9999, 10, 36)]
    public void SumOfDigitsInBase_Int_ReturnsExpectedSum(int value, int baseSystem, int expected)
    {
        // Act
        int result = SumOfDigits.SumOfDigitsInBase(value, baseSystem);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void SumOfDigitsInBase_IntMinValue_DoesNotThrow()
    {
        // Act
        int result = SumOfDigits.SumOfDigitsInBase(int.MinValue, 10);

        // Assert: digit sum of 2147483648
        Assert.Equal(2 + 1 + 4 + 7 + 4 + 8 + 3 + 6 + 4 + 8, result);
    }

    [Fact]
    public void SumOfDigitsInBase_LongMinValue_DoesNotThrow()
    {
        // Act
        int result = SumOfDigits.SumOfDigitsInBase(long.MinValue, 10);

        // Assert: digit sum of 9223372036854775808
        Assert.Equal(9 + 2 + 2 + 3 + 3 + 7 + 2 + 0 + 3 + 6 + 8 + 5 + 4 + 7 + 7 + 5 + 8 + 0 + 8, result);
    }

    [Fact]
    public void SumOfHexDigits_Byte_ReturnsExpectedSum()
    {
        // Act
        int result = SumOfDigits.SumOfHexDigits((byte)0xFF);

        // Assert: 0xF + 0xF = 30
        Assert.Equal(30, result);
    }
}
