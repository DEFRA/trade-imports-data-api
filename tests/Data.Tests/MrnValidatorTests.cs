using Defra.TradeImportsDataApi.Data.Extensions;
using FluentAssertions;

namespace Defra.TradeImportsDataApi.Data.Tests;

public class MrnValidatorTests
{
    [Theory]
    [InlineData("25GBEINIDEZXWQ2SAA", true)]
    [InlineData("25gbeinidezxwq2saa", true)]
    [InlineData("INVALID", false)]
    public void TestMrns(string mrn, bool valid)
    {
        // Arrange
        var validator = new MrnValidator();

        // Act
        var result = validator.IsValid(mrn);

        // Assert
        result.Should().Be(valid);
    }
}
