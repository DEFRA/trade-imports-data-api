using Defra.TradeImportsDataApi.Data.Entities;
using Defra.TradeImportsDataApi.Domain.Gvms;
using FluentAssertions;

namespace Defra.TradeImportsDataApi.Data.Tests.Entities;

public class GmrEntityTests
{
    [Fact]
    public void OnSave_RemovesDuplicates()
    {
        var subject = new GmrEntity
        {
            Id = "id",
            Gmr = new Gmr
            {
                Declarations = new Declarations
                {
                    Transits = [new Transits { Id = "mrn1" }],
                    Customs = [new Customs { Id = "mrn1" }, new Customs { Id = "mrn2" }],
                },
            },
        };

        subject.OnSave();

        subject.CustomsDeclarationIdentifiers.Should().BeEquivalentTo("mrn1", "mrn2");
    }
}
