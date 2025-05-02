using Defra.TradeImportsDataApi.Domain.Ipaffs.Constants;
using FluentAssertions;

namespace Defra.TradeImportsDataApi.Domain.Tests.Ipaffs.Constants;

public class ConsignmentDecisionTests
{
    [Theory]
    [InlineData("Non Acceptable", true)]
    [InlineData("non acceptable", true)]
    [InlineData(null, false)]
    public void WhenNonAcceptable_ThenMatch(string? status, bool expected)
    {
        ConsignmentDecision.IsNonAcceptable(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("Acceptable for Internal Market", true)]
    [InlineData("acceptable for internal market", true)]
    [InlineData(null, false)]
    public void WhenAcceptableForInternalMarket_ThenMatch(string? status, bool expected)
    {
        ConsignmentDecision.IsAcceptableForInternalMarket(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("Acceptable if Channeled", true)]
    [InlineData("acceptable if channeled", true)]
    [InlineData(null, false)]
    public void WhenAcceptableIfChanneled_ThenMatch(string? status, bool expected)
    {
        ConsignmentDecision.IsAcceptableIfChanneled(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("Acceptable for Transhipment", true)]
    [InlineData("acceptable for transhipment", true)]
    [InlineData(null, false)]
    public void WhenAcceptableForTranshipment_ThenMatch(string? status, bool expected)
    {
        ConsignmentDecision.IsAcceptableForTranshipment(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("Acceptable for Transit", true)]
    [InlineData("acceptable for transit", true)]
    [InlineData(null, false)]
    public void WhenAcceptableForTransit_ThenMatch(string? status, bool expected)
    {
        ConsignmentDecision.IsAcceptableForTransit(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("Acceptable for Temporary Import", true)]
    [InlineData("acceptable for temporary import", true)]
    [InlineData(null, false)]
    public void WhenAcceptableForTemporaryImport_ThenMatch(string? status, bool expected)
    {
        ConsignmentDecision.IsAcceptableForTemporaryImport(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("Acceptable for Specific Warehouse", true)]
    [InlineData("acceptable for specific warehouse", true)]
    [InlineData(null, false)]
    public void WhenAcceptableForSpecificWarehouse_ThenMatch(string? status, bool expected)
    {
        ConsignmentDecision.IsAcceptableForSpecificWarehouse(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("Acceptable for Private Import", true)]
    [InlineData("acceptable for private import", true)]
    [InlineData(null, false)]
    public void WhenAcceptableForPrivateImport_ThenMatch(string? status, bool expected)
    {
        ConsignmentDecision.IsAcceptableForPrivateImport(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("Acceptable for Transfer", true)]
    [InlineData("acceptable for transfer", true)]
    [InlineData(null, false)]
    public void WhenAcceptableForTransfer_ThenMatch(string? status, bool expected)
    {
        ConsignmentDecision.IsAcceptableForTransfer(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("Horse Re-entry", true)]
    [InlineData("horse re-entry", true)]
    [InlineData(null, false)]
    public void WhenHorseReEntry_ThenMatch(string? status, bool expected)
    {
        ConsignmentDecision.IsHorseReEntry(status).Should().Be(expected);
    }
}
