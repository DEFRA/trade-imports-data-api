using Defra.TradeImportsDataApi.Domain.Ipaffs.Constants;
using FluentAssertions;

namespace Defra.TradeImportsDataApi.Domain.Tests.Ipaffs.Constants;

public class AdditionalPermanentAddressesTypeTests
{
    [Theory]
    [InlineData("consignee", true)]
    [InlineData("CONSIGNEE", true)]
    [InlineData(null, false)]
    public void WhenConsignee_ThenMatch(string? status, bool expected)
    {
        AdditionalPermanentAddressesType.IsConsignee(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("destination", true)]
    [InlineData("DESTINATION", true)]
    [InlineData(null, false)]
    public void WhenDestination_ThenMatch(string? status, bool expected)
    {
        AdditionalPermanentAddressesType.IsDestination(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("exporter", true)]
    [InlineData("EXPORTER", true)]
    [InlineData(null, false)]
    public void WhenExporter_ThenMatch(string? status, bool expected)
    {
        AdditionalPermanentAddressesType.IsExporter(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("importer", true)]
    [InlineData("IMPORTER", true)]
    [InlineData(null, false)]
    public void WhenImporter_ThenMatch(string? status, bool expected)
    {
        AdditionalPermanentAddressesType.IsImporter(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("charity", true)]
    [InlineData("CHARITY", true)]
    [InlineData(null, false)]
    public void WhenCharity_ThenMatch(string? status, bool expected)
    {
        AdditionalPermanentAddressesType.IsCharity(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("commercial transporter", true)]
    [InlineData("COMMERCIAL TRANSPORTER", true)]
    [InlineData(null, false)]
    public void WhenCommercialTransporter_ThenMatch(string? status, bool expected)
    {
        AdditionalPermanentAddressesType.IsCommercialTransporter(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("commercial transporter - user added", true)]
    [InlineData("COMMERCIAL TRANSPORTER - USER ADDED", true)]
    [InlineData(null, false)]
    public void WhenCommercialTransporterUserAdded_ThenMatch(string? status, bool expected)
    {
        AdditionalPermanentAddressesType.IsCommercialTransporterUserAdded(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("private transporter", true)]
    [InlineData("PRIVATE TRANSPORTER", true)]
    [InlineData(null, false)]
    public void WhenPrivateTransporter_ThenMatch(string? status, bool expected)
    {
        AdditionalPermanentAddressesType.IsPrivateTransporter(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("temporary address", true)]
    [InlineData("TEMPORARY ADDRESS", true)]
    [InlineData(null, false)]
    public void WhenTemporaryAddress_ThenMatch(string? status, bool expected)
    {
        AdditionalPermanentAddressesType.IsTemporaryAddress(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("premises of origin", true)]
    [InlineData("PREMISES OF ORIGIN", true)]
    [InlineData(null, false)]
    public void WhenPremisesOfOrigin_ThenMatch(string? status, bool expected)
    {
        AdditionalPermanentAddressesType.IsPremisesOfOrigin(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("organisation branch address", true)]
    [InlineData("ORGANISATION BRANCH ADDRESS", true)]
    [InlineData(null, false)]
    public void WhenOrganisationBranchAddress_ThenMatch(string? status, bool expected)
    {
        AdditionalPermanentAddressesType.IsOrganisationBranchAddress(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("packer", true)]
    [InlineData("PACKER", true)]
    [InlineData(null, false)]
    public void WhenPacker_ThenMatch(string? status, bool expected)
    {
        AdditionalPermanentAddressesType.IsPacker(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("pod", true)]
    [InlineData("POD", true)]
    [InlineData(null, false)]
    public void WhenPod_ThenMatch(string? status, bool expected)
    {
        AdditionalPermanentAddressesType.IsPod(status).Should().Be(expected);
    }
}
