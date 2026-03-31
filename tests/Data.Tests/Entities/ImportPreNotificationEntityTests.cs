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
                    new ExternalReference() { Reference = "ABC123" },
                    new ExternalReference() { Reference = "abc123" },
                    new ExternalReference() { Reference = "CBA123" },
                ],
            },
        };

        subject.OnSave();

        subject.Tags.Should().BeEquivalentTo("abc123", "cba123");
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
