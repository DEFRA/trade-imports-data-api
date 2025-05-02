using Defra.TradeImportsDataApi.Domain.Ipaffs.Constants;
using FluentAssertions;

namespace Defra.TradeImportsDataApi.Domain.Tests.Ipaffs.Constants;

public class PartyTypeTests
{
    [Theory]
    [InlineData("Commercial transporter", true)]
    [InlineData("commercial transporter", true)]
    [InlineData(null, false)]
    public void WhenCommercialTransporter_ThenMatch(string? status, bool expected)
    {
        PartyType.IsCommercialTransporter(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("Private transporter", true)]
    [InlineData("private transporter", true)]
    [InlineData(null, false)]
    public void WhenPrivateTransporter_ThenMatch(string? status, bool expected)
    {
        PartyType.IsPrivateTransporter(status).Should().Be(expected);
    }
}
