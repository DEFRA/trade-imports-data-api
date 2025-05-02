using Defra.TradeImportsDataApi.Domain.Ipaffs.Constants;
using FluentAssertions;

namespace Defra.TradeImportsDataApi.Domain.Tests.Ipaffs.Constants;

public class PersonResponsibleTypeTests
{
    [Theory]
    [InlineData("Commercial transporter", true)]
    [InlineData("commercial transporter", true)]
    [InlineData(null, false)]
    public void WhenCommercialTransporter_ThenMatch(string? status, bool expected)
    {
        PersonResponsibleType.IsCommercialTransporter(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("Private transporter", true)]
    [InlineData("private transporter", true)]
    [InlineData(null, false)]
    public void WhenPrivateTransporter_ThenMatch(string? status, bool expected)
    {
        PersonResponsibleType.IsPrivateTransporter(status).Should().Be(expected);
    }
}
