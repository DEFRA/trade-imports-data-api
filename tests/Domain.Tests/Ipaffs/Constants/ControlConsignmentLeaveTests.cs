using Defra.TradeImportsDataApi.Domain.Ipaffs.Constants;
using FluentAssertions;

namespace Defra.TradeImportsDataApi.Domain.Tests.Ipaffs.Constants;

public class ControlConsignmentLeaveTests
{
    [Theory]
    [InlineData("YES", true)]
    [InlineData("yes", true)]
    [InlineData(null, false)]
    public void WhenYes_ThenMatch(string? status, bool expected)
    {
        ControlConsignmentLeave.IsYes(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("NO", true)]
    [InlineData("no", true)]
    [InlineData(null, false)]
    public void WhenNo_ThenMatch(string? status, bool expected)
    {
        ControlConsignmentLeave.IsNo(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("It has been destroyed", true)]
    [InlineData("it has been destroyed", true)]
    [InlineData(null, false)]
    public void WhenItHasBeenDestroyed_ThenMatch(string? status, bool expected)
    {
        ControlConsignmentLeave.IsItHasBeenDestroyed(status).Should().Be(expected);
    }
}
