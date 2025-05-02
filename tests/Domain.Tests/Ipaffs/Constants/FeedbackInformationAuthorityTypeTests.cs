using Defra.TradeImportsDataApi.Domain.Ipaffs.Constants;
using FluentAssertions;

namespace Defra.TradeImportsDataApi.Domain.Tests.Ipaffs.Constants;

public class FeedbackInformationAuthorityTypeTests
{
    [Theory]
    [InlineData("exitbip", true)]
    [InlineData("EXITBIP", true)]
    [InlineData(null, false)]
    public void WhenExitBip_ThenMatch(string? status, bool expected)
    {
        FeedbackInformationAuthorityType.IsExitBip(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("finalbip", true)]
    [InlineData("FINALBIP", true)]
    [InlineData(null, false)]
    public void WhenFinalBip_ThenMatch(string? status, bool expected)
    {
        FeedbackInformationAuthorityType.IsFinalBip(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("localvetunit", true)]
    [InlineData("LOCALVETUNIT", true)]
    [InlineData(null, false)]
    public void WhenLocalVetUnit_ThenMatch(string? status, bool expected)
    {
        FeedbackInformationAuthorityType.IsLocalVetUnit(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("inspunit", true)]
    [InlineData("INSPUNIT", true)]
    [InlineData(null, false)]
    public void WhenInspUnit_ThenMatch(string? status, bool expected)
    {
        FeedbackInformationAuthorityType.IsInspUnit(status).Should().Be(expected);
    }
}
