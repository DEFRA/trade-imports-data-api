using Defra.TradeImportsDataApi.Domain.Ipaffs.Constants;
using FluentAssertions;

namespace Defra.TradeImportsDataApi.Domain.Tests.Ipaffs.Constants;

public class FeedbackInformationAuthorityTypeTests
{
    [Theory]
    [InlineData("exitbip", true)]
    [InlineData("EXITBIP", true)]
    [InlineData(null, false)]
    public void WhenExitbip_ThenMatch(string? status, bool expected)
    {
        FeedbackInformationAuthorityType.IsExitbip(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("finalbip", true)]
    [InlineData("FINALBIP", true)]
    [InlineData(null, false)]
    public void WhenFinalbip_ThenMatch(string? status, bool expected)
    {
        FeedbackInformationAuthorityType.IsFinalbip(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("localvetunit", true)]
    [InlineData("LOCALVETUNIT", true)]
    [InlineData(null, false)]
    public void WhenLocalvetunit_ThenMatch(string? status, bool expected)
    {
        FeedbackInformationAuthorityType.IsLocalvetunit(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("inspunit", true)]
    [InlineData("INSPUNIT", true)]
    [InlineData(null, false)]
    public void WhenInspunit_ThenMatch(string? status, bool expected)
    {
        FeedbackInformationAuthorityType.IsInspunit(status).Should().Be(expected);
    }
}
