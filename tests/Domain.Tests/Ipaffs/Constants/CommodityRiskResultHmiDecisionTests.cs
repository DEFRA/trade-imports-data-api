using Defra.TradeImportsDataApi.Domain.Ipaffs.Constants;
using FluentAssertions;

namespace Defra.TradeImportsDataApi.Domain.Tests.Ipaffs.Constants;

public class CommodityRiskResultHmiDecisionTests
{
    [Theory]
    [InlineData("REQUIRED", true)]
    [InlineData("required", true)]
    [InlineData(null, false)]
    public void WhenRequired_ThenMatch(string? status, bool expected)
    {
        CommodityRiskResultHmiDecision.IsRequired(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("NOTREQUIRED", true)]
    [InlineData("notrequired", true)]
    [InlineData(null, false)]
    public void WhenNotRequired_ThenMatch(string? status, bool expected)
    {
        CommodityRiskResultHmiDecision.IsNotRequired(status).Should().Be(expected);
    }
}
