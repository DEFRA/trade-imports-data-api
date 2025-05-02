using Defra.TradeImportsDataApi.Domain.Ipaffs.Constants;
using FluentAssertions;

namespace Defra.TradeImportsDataApi.Domain.Tests.Ipaffs.Constants;

public class PurposePurposeGroupTests
{
    [Theory]
    [InlineData("For Import", true)]
    [InlineData("for import", true)]
    [InlineData(null, false)]
    public void WhenForImport_ThenMatch(string? status, bool expected)
    {
        PurposePurposeGroup.IsForImport(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("For NON-Conforming Consignments", true)]
    [InlineData("for non-conforming consignments", true)]
    [InlineData(null, false)]
    public void WhenForNonConformingConsignments_ThenMatch(string? status, bool expected)
    {
        PurposePurposeGroup.IsForNonConformingConsignments(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("For Transhipment to", true)]
    [InlineData("for transhipment to", true)]
    [InlineData(null, false)]
    public void WhenForTranshipmentTo_ThenMatch(string? status, bool expected)
    {
        PurposePurposeGroup.IsForTranshipmentTo(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("For Transit to 3rd Country", true)]
    [InlineData("for transit to 3rd country", true)]
    [InlineData(null, false)]
    public void WhenForTransitTo3rdCountry_ThenMatch(string? status, bool expected)
    {
        PurposePurposeGroup.IsForTransitTo3rdCountry(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("For Re-Import", true)]
    [InlineData("for re-import", true)]
    [InlineData(null, false)]
    public void WhenForReImport_ThenMatch(string? status, bool expected)
    {
        PurposePurposeGroup.IsForReImport(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("For Private Import", true)]
    [InlineData("for private import", true)]
    [InlineData(null, false)]
    public void WhenForPrivateImport_ThenMatch(string? status, bool expected)
    {
        PurposePurposeGroup.IsForPrivateImport(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("For Transfer To", true)]
    [InlineData("for transfer to", true)]
    [InlineData(null, false)]
    public void WhenForTransferTo_ThenMatch(string? status, bool expected)
    {
        PurposePurposeGroup.IsForTransferTo(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("For Import Re-Conformity Check", true)]
    [InlineData("for import re-conformity check", true)]
    [InlineData(null, false)]
    public void WhenForImportReConformityCheck_ThenMatch(string? status, bool expected)
    {
        PurposePurposeGroup.IsForImportReConformityCheck(status).Should().Be(expected);
    }
}
