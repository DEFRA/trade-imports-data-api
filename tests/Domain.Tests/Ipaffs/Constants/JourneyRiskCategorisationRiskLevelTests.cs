using Defra.TradeImportsDataApi.Domain.Ipaffs.Constants;
using FluentAssertions;

namespace Defra.TradeImportsDataApi.Domain.Tests.Ipaffs.Constants;

public class JourneyRiskCategorisationRiskLevelTests
{
    [Theory]
    [InlineData("High", true)]
    [InlineData("high", true)]
    [InlineData(null, false)]
    public void WhenHigh_ThenMatch(string? status, bool expected)
    {
        JourneyRiskCategorisationRiskLevel.IsHigh(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("Medium", true)]
    [InlineData("medium", true)]
    [InlineData(null, false)]
    public void WhenMedium_ThenMatch(string? status, bool expected)
    {
        JourneyRiskCategorisationRiskLevel.IsMedium(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("Low", true)]
    [InlineData("low", true)]
    [InlineData(null, false)]
    public void WhenLow_ThenMatch(string? status, bool expected)
    {
        JourneyRiskCategorisationRiskLevel.IsLow(status).Should().Be(expected);
    }
}
