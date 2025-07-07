using Defra.TradeImportsDataApi.Domain.Ipaffs;
using FluentAssertions;

namespace Defra.TradeImportsDataApi.Api.Tests.Domain;

public class ChedIdReferenceTests
{
    [Theory]
    [InlineData("CHEDA.GB.2025.1234567", "1234567")]
    [InlineData("CHEDA.GB.2025.1234567R", "1234567R")]
    [InlineData("CHEDA.GB.2025.12345678", "2345678")]
    public void ValidTests(string chedId, string identifier)
    {
        new ChedIdReference(chedId).GetIdentifier().Should().Be(identifier);
    }

    [Theory]
    [InlineData("INVALID")]
    [InlineData("CHEDA.GB.2025.123456")]
    public void InValidTests(string chedId)
    {
        Action act = () => new ChedIdReference(chedId).GetIdentifier();

        act.Should().Throw<FormatException>();
    }
}
