using Defra.TradeImportsDataApi.Domain.CustomsDeclaration;
using FluentAssertions;

namespace Defra.TradeImportsDataApi.Api.Tests.Domain;

public class ImportDocumentReferenceTests
{
    [Theory]
    [InlineData("C640", true)]
    [InlineData("C678", true)]
    [InlineData("N853", true)]
    [InlineData("N851", true)]
    [InlineData("9115", true)]
    [InlineData("C085", true)]
    [InlineData("N002", true)]
    [InlineData("Invalid", false)]
    public void IsValid(string documentCode, bool valid)
    {
        ImportDocumentReference.IsValid(documentCode).Should().Be(valid);
    }

    [Theory]
    [InlineData("CHEDA.GB.2025.1234567", "C640", "1234567")]
    [InlineData("CHEDA.GB.2025.12345678", "C640", "1234567")]
    [InlineData("CHEDA.GB.2025.123456", "C678", null)]
    public void GetIdentifier(string documentReference, string documentCode, string? identifier)
    {
        if (string.IsNullOrEmpty(identifier))
        {
            identifier = string.Empty;
        }
        new ImportDocumentReference(documentReference).GetIdentifier(documentCode).Should().Be(identifier);
    }
}
