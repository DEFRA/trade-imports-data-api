using Defra.TradeImportsDataApi.Domain.Ipaffs.Constants;
using FluentAssertions;

namespace Defra.TradeImportsDataApi.Domain.Tests.Ipaffs.Constants;

public class CommodityResultsExitRiskDecisionTests
{
    [Theory]
    [InlineData("REQUIRED", true)]
    [InlineData("required", true)]
    [InlineData(null, false)]
    public void WhenRequired_ThenMatch(string? status, bool expected)
    {
        CommodityResultsExitRiskDecision.IsRequired(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("NOTREQUIRED", true)]
    [InlineData("notrequired", true)]
    [InlineData(null, false)]
    public void WhenNotrequired_ThenMatch(string? status, bool expected)
    {
        CommodityResultsExitRiskDecision.IsNotrequired(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("INCONCLUSIVE", true)]
    [InlineData("inconclusive", true)]
    [InlineData(null, false)]
    public void WhenInconclusive_ThenMatch(string? status, bool expected)
    {
        CommodityResultsExitRiskDecision.IsInconclusive(status).Should().Be(expected);
    }
}
