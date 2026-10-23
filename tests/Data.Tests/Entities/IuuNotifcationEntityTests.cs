using Defra.TradeImportsDataApi.Data.Entities;
using Defra.TradeImportsDataApi.Domain.Iuu;
using FluentAssertions;

namespace Defra.TradeImportsDataApi.Data.Tests.Entities;

public class IuuNotifcationEntityTests
{
    [Fact]
    public void OnSave_ShouldSetTags()
    {
        var subject = new IuuNotificationEntity
        {
            Id = "IUU.123",
            IuuNotification = new IuuNotification
            {
                Reference = "IUU.123",
                LastUpdated = new DateTime(2026, 10, 16, 9, 0, 0, DateTimeKind.Utc),
                IuuOption = "IUUOK",
            },
        };

        subject.OnSave();

        subject.Tags.Should().BeEquivalentTo("iuu.123");
    }
}
