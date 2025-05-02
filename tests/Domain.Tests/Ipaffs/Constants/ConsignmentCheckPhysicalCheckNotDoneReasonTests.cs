using Defra.TradeImportsDataApi.Domain.Ipaffs.Constants;
using FluentAssertions;

namespace Defra.TradeImportsDataApi.Domain.Tests.Ipaffs.Constants;

public class ConsignmentCheckPhysicalCheckNotDoneReasonTests
{
    [Theory]
    [InlineData("Reduced checks regime", true)]
    [InlineData("reduced checks regime", true)]
    [InlineData(null, false)]
    public void WhenReducedChecksRegime_ThenMatch(string? status, bool expected)
    {
        ConsignmentCheckPhysicalCheckNotDoneReason.IsReducedChecksRegime(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("Other", true)]
    [InlineData("other", true)]
    [InlineData(null, false)]
    public void WhenOther_ThenMatch(string? status, bool expected)
    {
        ConsignmentCheckPhysicalCheckNotDoneReason.IsOther(status).Should().Be(expected);
    }
}
