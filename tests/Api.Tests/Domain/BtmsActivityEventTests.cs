using Defra.TradeImportsDataApi.Domain.Events;
using FluentAssertions;

namespace Defra.TradeImportsDataApi.Api.Tests.Domain;

public class BtmsActivityEventTests
{
    [Fact]
    public void Create_WithRequiredProperties_SetsPropertiesAndTimestampRecent()
    {
        var before = DateTime.UtcNow;
        var activity = new BtmsToCdsActivity
        {
            CorrelationId = "123",
            StatusCode = 200,
            ResponseTimestamp = before,
        };

        var evt = new BtmsActivityEvent<BtmsToCdsActivity>
        {
            ServiceName = "svc",
            ResourceId = "r1",
            ResourceType = "type",
            SubResourceType = "sub-json",
            Activity = activity,
        };

        evt.ServiceName.Should().Be("svc");
        evt.ResourceId.Should().Be("r1");
        evt.ResourceType.Should().Be("type");
        evt.SubResourceType.Should().Be("sub-json");
        evt.Activity.CorrelationId.Should().Be(activity.CorrelationId);
        evt.Activity.StatusCode.Should().Be(activity.StatusCode);
        evt.Activity.ResponseTimestamp.Should().Be(activity.ResponseTimestamp);

        // Timestamp should be set to something very close to now (Utc)
        evt.Timestamp.Should().BeOnOrAfter(before);
        evt.Timestamp.Should().BeOnOrBefore(DateTime.UtcNow.AddSeconds(5));
    }
}
