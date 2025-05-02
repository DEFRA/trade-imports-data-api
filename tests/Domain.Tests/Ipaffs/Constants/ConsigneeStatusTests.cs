using Defra.TradeImportsDataApi.Domain.Ipaffs.Constants;
using FluentAssertions;

namespace Defra.TradeImportsDataApi.Domain.Tests.Ipaffs.Constants;

public class ConsigneeStatusTests
{
    [Theory]
    [InlineData("approved", true)]
    [InlineData("APPROVED", true)]
    [InlineData(null, false)]
    public void WhenApproved_ThenMatch(string? status, bool expected)
    {
        ConsigneeStatus.IsApproved(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("nonapproved", true)]
    [InlineData("NONAPPROVED", true)]
    [InlineData(null, false)]
    public void WhenNonapproved_ThenMatch(string? status, bool expected)
    {
        ConsigneeStatus.IsNonapproved(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("suspended", true)]
    [InlineData("SUSPENDED", true)]
    [InlineData(null, false)]
    public void WhenSuspended_ThenMatch(string? status, bool expected)
    {
        ConsigneeStatus.IsSuspended(status).Should().Be(expected);
    }
}
