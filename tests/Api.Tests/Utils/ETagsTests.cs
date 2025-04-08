using Defra.TradeImportsDataApi.Api.Utils;
using FluentAssertions;

namespace Defra.TradeImportsDataApi.Api.Tests.Utils;

public class ETagsTests
{
    [Fact]
    public void ValidateAndParseFirst_WhenIfMatchNull_ShouldReturnNull()
    {
        var result = ETags.ValidateAndParseFirst(null);

        result.Should().BeNull();
    }

    [Fact]
    public void ValidateAndParseFirst_WhenIfMatchEmpty_ShouldReturnNull()
    {
        var result = ETags.ValidateAndParseFirst("");

        result.Should().BeNull();
    }

    [Theory]
    [InlineData("invalid")]
    [InlineData("etag1, etag2")]
    [InlineData("\"etag1\", \"etag2\"")]
    public void ValidateAndParseFirst_WhenIfMatchInvalid_ShouldThrow(string ifMatch)
    {
        var act = () => ETags.ValidateAndParseFirst(ifMatch);

        act.Should().Throw<FormatException>();
    }

    [Theory]
    [InlineData("\"etag\"", "etag")]
    public void ValidateAndParseFirst_WhenIfMatchValid_ShouldReturnFirst(string ifMatch, string expected)
    {
        var result = ETags.ValidateAndParseFirst(ifMatch);

        result.Should().Be(expected);
    }
}
