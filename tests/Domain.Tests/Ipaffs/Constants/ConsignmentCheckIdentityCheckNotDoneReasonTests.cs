using Defra.TradeImportsDataApi.Domain.Ipaffs.Constants;
using FluentAssertions;

namespace Defra.TradeImportsDataApi.Domain.Tests.Ipaffs.Constants;

public class ConsignmentCheckIdentityCheckNotDoneReasonTests
{
    [Theory]
    [InlineData("Reduced checks regime", true)]
    [InlineData("reduced checks regime", true)]
    [InlineData(null, false)]
    public void WhenReducedChecksRegime_ThenMatch(string? status, bool expected)
    {
        ConsignmentCheckIdentityCheckNotDoneReason.IsReducedChecksRegime(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("Not required", true)]
    [InlineData("not required", true)]
    [InlineData(null, false)]
    public void WhenNotRequired_ThenMatch(string? status, bool expected)
    {
        ConsignmentCheckIdentityCheckNotDoneReason.IsNotRequired(status).Should().Be(expected);
    }
}
