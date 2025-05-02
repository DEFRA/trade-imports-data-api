using Defra.TradeImportsDataApi.Domain.Ipaffs.Constants;
using FluentAssertions;

namespace Defra.TradeImportsDataApi.Domain.Tests.Ipaffs.Constants;

public class InspectionCheckTypeTests
{
    [Theory]
    [InlineData("PHSI_DOCUMENT", true)]
    [InlineData("phsi_document", true)]
    [InlineData(null, false)]
    public void WhenPhsiDocument_ThenMatch(string? status, bool expected)
    {
        InspectionCheckType.IsPhsiDocument(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("PHSI_IDENTITY", true)]
    [InlineData("phsi_identity", true)]
    [InlineData(null, false)]
    public void WhenPhsiIdentity_ThenMatch(string? status, bool expected)
    {
        InspectionCheckType.IsPhsiIdentity(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("PHSI_PHYSICAL", true)]
    [InlineData("phsi_physical", true)]
    [InlineData(null, false)]
    public void WhenPhsiPhysical_ThenMatch(string? status, bool expected)
    {
        InspectionCheckType.IsPhsiPhysical(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("HMI", true)]
    [InlineData("hmi", true)]
    [InlineData(null, false)]
    public void WhenHmi_ThenMatch(string? status, bool expected)
    {
        InspectionCheckType.IsHmi(status).Should().Be(expected);
    }
}
