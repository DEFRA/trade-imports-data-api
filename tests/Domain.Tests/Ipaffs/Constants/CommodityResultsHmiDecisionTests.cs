using Defra.TradeImportsDataApi.Domain.Ipaffs.Constants;
using FluentAssertions;

namespace Defra.TradeImportsDataApi.Domain.Tests.Ipaffs.Constants;

public class CommodityResultsHmiDecisionTests
{
    [Theory]
    [InlineData("REQUIRED", true)]
    [InlineData("required", true)]
    [InlineData(null, false)]
    public void WhenRequired_ThenMatch(string? status, bool expected)
    {
        CommodityResultsHmiDecision.IsRequired(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("NOTREQUIRED", true)]
    [InlineData("notrequired", true)]
    [InlineData(null, false)]
    public void WhenNotrequired_ThenMatch(string? status, bool expected)
    {
        CommodityResultsHmiDecision.IsNotrequired(status).Should().Be(expected);
    }
}
