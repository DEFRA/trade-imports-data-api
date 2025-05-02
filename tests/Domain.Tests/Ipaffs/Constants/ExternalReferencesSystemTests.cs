using Defra.TradeImportsDataApi.Domain.Ipaffs.Constants;
using FluentAssertions;

namespace Defra.TradeImportsDataApi.Domain.Tests.Ipaffs.Constants;

public class ExternalReferencesSystemTests
{
    [Theory]
    [InlineData("E-CERT", true)]
    [InlineData("e-cert", true)]
    [InlineData(null, false)]
    public void WhenECert_ThenMatch(string? status, bool expected)
    {
        ExternalReferencesSystem.IsECert(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("E-PHYTO", true)]
    [InlineData("e-phyto", true)]
    [InlineData(null, false)]
    public void WhenEPhyto_ThenMatch(string? status, bool expected)
    {
        ExternalReferencesSystem.IsEPhyto(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("E-NOTIFICATION", true)]
    [InlineData("e-notification", true)]
    [InlineData(null, false)]
    public void WhenENotification_ThenMatch(string? status, bool expected)
    {
        ExternalReferencesSystem.IsENotification(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("NCTS", true)]
    [InlineData("ncts", true)]
    [InlineData(null, false)]
    public void WhenNcts_ThenMatch(string? status, bool expected)
    {
        ExternalReferencesSystem.IsNcts(status).Should().Be(expected);
    }
}
