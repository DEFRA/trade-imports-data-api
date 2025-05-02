using Defra.TradeImportsDataApi.Domain.CustomsDeclaration;
using Defra.TradeImportsDataApi.Domain.Ipaffs;
using Defra.TradeImportsDataApi.Domain.Ipaffs.Constants;
using FluentAssertions;

// ReSharper disable InconsistentNaming

namespace Defra.TradeImportsDataApi.Api.Tests.Domain;

public class ImportDocumentExtensionsTests
{
    [Theory]
    [InlineData("9115", NotificationType.Chedpp)]
    [InlineData("C633", NotificationType.Chedpp)]
    [InlineData("N002", NotificationType.Chedpp)]
    [InlineData("N851", NotificationType.Chedpp)]
    [InlineData("C085", NotificationType.Chedpp)]
    [InlineData("N852", NotificationType.Ced)]
    [InlineData("C678", NotificationType.Ced)]
    [InlineData("C640", NotificationType.Cveda)]
    [InlineData("C641", NotificationType.Cvedp)]
    [InlineData("C673", NotificationType.Cvedp)]
    [InlineData("N853", NotificationType.Cvedp)]
    [InlineData("9HCG", null)]
    [InlineData("INVALID", null)]
    public void GetChedTypeTest(string documentCode, string? expectedType)
    {
        new ImportDocument { DocumentCode = documentCode }
            .GetChedType()
            .Should()
            .Be(expectedType);
    }
}
