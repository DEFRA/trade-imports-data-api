using Defra.TradeImportsDataApi.Data.Entities;
using Defra.TradeImportsDataApi.Domain.Ipaffs;
using FluentAssertions;

namespace Defra.TradeImportsDataApi.Data.Tests.Entities;

public class ImportPreNotificationEntityTests
{
    [Fact]
    public void OnSave_CopiesExternalReferenceToTags()
    {
        var subject = new ImportPreNotificationEntity()
        {
            Id = "id.1234567",
            ImportPreNotification = new ImportPreNotification()
            {
                ExternalReferences =
                [
                    new ExternalReference() { System = "NCTS", Reference = "ABC123" },
                    new ExternalReference() { System = "NCTS", Reference = "abc123" },
                    new ExternalReference() { System = "NCTS", Reference = "CBA123" },
                    new ExternalReference() { System = "NCTS", Reference = "25GBEINIDEZXWQ2SA4" },
                    new ExternalReference() { System = "NCTS", Reference = "25gbeinidezxwq2sa4" },
                    new ExternalReference() { System = "OTHER", Reference = "25gbeinidezxwq2sa5" },
                ],
            },
        };

        subject.OnSave();

        subject.Tags.Should().BeEquivalentTo("25gbeinidezxwq2sa4");
    }

    [Fact]
    public void OnSave_WhenExternalReferenceIsNull()
    {
        var subject = new ImportPreNotificationEntity()
        {
            Id = "id.1234567",
            ImportPreNotification = new ImportPreNotification() { ExternalReferences = null },
        };

        subject.OnSave();

        subject.Tags.Should().BeEmpty();
    }

    [Fact]
    public void OnSave_WhenExternalReferenceValueIsNull()
    {
        var subject = new ImportPreNotificationEntity()
        {
            Id = "id.1234567",
            ImportPreNotification = new ImportPreNotification()
            {
                ExternalReferences = [new ExternalReference() { Reference = null }],
            },
        };

        subject.OnSave();

        subject.Tags.Should().BeEmpty();
    }
}
