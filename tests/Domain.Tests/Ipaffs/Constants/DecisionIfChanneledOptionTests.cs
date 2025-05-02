using Defra.TradeImportsDataApi.Domain.Ipaffs.Constants;
using FluentAssertions;

namespace Defra.TradeImportsDataApi.Domain.Tests.Ipaffs.Constants;

public class DecisionIfChanneledOptionTests
{
    [Theory]
    [InlineData("article8", true)]
    [InlineData("ARTICLE8", true)]
    [InlineData(null, false)]
    public void WhenArticle8_ThenMatch(string? status, bool expected)
    {
        DecisionIfChanneledOption.IsArticle8(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("article15", true)]
    [InlineData("ARTICLE15", true)]
    [InlineData(null, false)]
    public void WhenArticle15_ThenMatch(string? status, bool expected)
    {
        DecisionIfChanneledOption.IsArticle15(status).Should().Be(expected);
    }
}
