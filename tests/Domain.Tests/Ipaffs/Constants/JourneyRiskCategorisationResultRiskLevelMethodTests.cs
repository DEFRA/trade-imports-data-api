using Defra.TradeImportsDataApi.Domain.Ipaffs.Constants;
using FluentAssertions;

namespace Defra.TradeImportsDataApi.Domain.Tests.Ipaffs.Constants;

public class JourneyRiskCategorisationResultRiskLevelMethodTests
{
    [Theory]
    [InlineData("System", true)]
    [InlineData("system", true)]
    [InlineData(null, false)]
    public void WhenSystem_ThenMatch(string? status, bool expected)
    {
        JourneyRiskCategorisationResultRiskLevelMethod.IsSystem(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("User", true)]
    [InlineData("user", true)]
    [InlineData(null, false)]
    public void WhenUser_ThenMatch(string? status, bool expected)
    {
        JourneyRiskCategorisationResultRiskLevelMethod.IsUser(status).Should().Be(expected);
    }
}
