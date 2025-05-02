using Defra.TradeImportsDataApi.Domain.Ipaffs.Constants;
using FluentAssertions;

namespace Defra.TradeImportsDataApi.Domain.Tests.Ipaffs.Constants;

public class DecisionDefinitiveImportPurposeTests
{
    [Theory]
    [InlineData("slaughter", true)]
    [InlineData("SLAUGHTER", true)]
    [InlineData(null, false)]
    public void WhenSlaughter_ThenMatch(string? status, bool expected)
    {
        DecisionDefinitiveImportPurpose.IsSlaughter(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("approvedbodies", true)]
    [InlineData("APPROVEDBODIES", true)]
    [InlineData(null, false)]
    public void WhenApprovedBodies_ThenMatch(string? status, bool expected)
    {
        DecisionDefinitiveImportPurpose.IsApprovedBodies(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("quarantine", true)]
    [InlineData("QUARANTINE", true)]
    [InlineData(null, false)]
    public void WhenQuarantine_ThenMatch(string? status, bool expected)
    {
        DecisionDefinitiveImportPurpose.IsQuarantine(status).Should().Be(expected);
    }
}
