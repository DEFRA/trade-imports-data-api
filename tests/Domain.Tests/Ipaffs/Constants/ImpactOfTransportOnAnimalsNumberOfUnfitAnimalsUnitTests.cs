using Defra.TradeImportsDataApi.Domain.Ipaffs.Constants;
using FluentAssertions;

namespace Defra.TradeImportsDataApi.Domain.Tests.Ipaffs.Constants;

public class ImpactOfTransportOnAnimalsNumberOfUnfitAnimalsUnitTests
{
    [Theory]
    [InlineData("percent", true)]
    [InlineData("PERCENT", true)]
    [InlineData(null, false)]
    public void WhenPercent_ThenMatch(string? status, bool expected)
    {
        ImpactOfTransportOnAnimalsNumberOfUnfitAnimalsUnit.IsPercent(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("number", true)]
    [InlineData("NUMBER", true)]
    [InlineData(null, false)]
    public void WhenNumber_ThenMatch(string? status, bool expected)
    {
        ImpactOfTransportOnAnimalsNumberOfUnfitAnimalsUnit.IsNumber(status).Should().Be(expected);
    }
}
