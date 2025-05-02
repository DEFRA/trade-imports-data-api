using Defra.TradeImportsDataApi.Domain.Ipaffs.Constants;
using FluentAssertions;

namespace Defra.TradeImportsDataApi.Domain.Tests.Ipaffs.Constants;

public class CommodityRiskResultRiskDecisionTests
{
    [Theory]
    [InlineData("REQUIRED", true)]
    [InlineData("required", true)]
    [InlineData(null, false)]
    public void WhenRequired_ThenMatch(string? status, bool expected)
    {
        CommodityRiskResultRiskDecision.IsRequired(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("NOTREQUIRED", true)]
    [InlineData("notrequired", true)]
    [InlineData(null, false)]
    public void WhenNotRequired_ThenMatch(string? status, bool expected)
    {
        CommodityRiskResultRiskDecision.IsNotRequired(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("INCONCLUSIVE", true)]
    [InlineData("inconclusive", true)]
    [InlineData(null, false)]
    public void WhenInconclusive_ThenMatch(string? status, bool expected)
    {
        CommodityRiskResultRiskDecision.IsInconclusive(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("REENFORCED_CHECK", true)]
    [InlineData("reenforced_check", true)]
    [InlineData(null, false)]
    public void WhenReEnforcedCheck_ThenMatch(string? status, bool expected)
    {
        CommodityRiskResultRiskDecision.IsReEnforcedCheck(status).Should().Be(expected);
    }
}
