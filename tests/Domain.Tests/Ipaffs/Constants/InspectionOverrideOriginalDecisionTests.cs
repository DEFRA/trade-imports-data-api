using Defra.TradeImportsDataApi.Domain.Ipaffs.Constants;
using FluentAssertions;

namespace Defra.TradeImportsDataApi.Domain.Tests.Ipaffs.Constants;

public class InspectionOverrideOriginalDecisionTests
{
    [Theory]
    [InlineData("Required", true)]
    [InlineData("required", true)]
    [InlineData(null, false)]
    public void WhenRequired_ThenMatch(string? status, bool expected)
    {
        InspectionOverrideOriginalDecision.IsRequired(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("Inconclusive", true)]
    [InlineData("inconclusive", true)]
    [InlineData(null, false)]
    public void WhenInconclusive_ThenMatch(string? status, bool expected)
    {
        InspectionOverrideOriginalDecision.IsInconclusive(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("Not required", true)]
    [InlineData("not required", true)]
    [InlineData(null, false)]
    public void WhenNotRequired_ThenMatch(string? status, bool expected)
    {
        InspectionOverrideOriginalDecision.IsNotRequired(status).Should().Be(expected);
    }
}
