using Defra.TradeImportsDataApi.Data.Entities;
using Defra.TradeImportsDataApi.Domain.CustomsDeclaration;
using FluentAssertions;

namespace Defra.TradeImportsDataApi.Data.Tests.Entities;

public class CustomsDeclarationEntityTests
{
    [Fact]
    public void OnSave_ShouldSetIuuNotificationIdentifiers()
    {
        var subject = new CustomsDeclarationEntity
        {
            Id = "MRN.123",
            ClearanceRequest = new ClearanceRequest
            {
                Commodities = new[]
                {
                    new Commodity
                    {
                        Documents = new[]
                        {
                            new ImportDocument
                            {
                                DocumentCode = "C673",
                                DocumentReference = new ImportDocumentReference("IUU.Reference.1"),
                            },
                            new ImportDocument
                            {
                                DocumentCode = "C641",
                                DocumentReference = new ImportDocumentReference("IUU.Reference.2"),
                            },
                            new ImportDocument
                            {
                                DocumentCode = "A123",
                                DocumentReference = new ImportDocumentReference("Not.IUU.Reference"),
                            },
                        },
                    },
                },
            },
        };

        subject.OnSave();

        subject.IuuNotificationIdentifiers.Should().BeEquivalentTo("IUU.Reference.1", "IUU.Reference.2");
    }
}
