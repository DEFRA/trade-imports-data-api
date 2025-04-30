using Defra.TradeImportsDataApi.Domain.Gvms;
using FluentAssertions;

namespace Defra.TradeImportsDataApi.Api.IntegrationTests.Endpoints;

public class GmrTests : IntegrationTestBase
{
    [Fact]
    public async Task WhenDoesNotExist_ShouldCreateAndRead()
    {
        var client = CreateDataApiClient();
        var gmrId = Guid.NewGuid().ToString("N");

        var result = await client.GetGmr(gmrId, CancellationToken.None);
        result.Should().BeNull();

        await client.PutGmr(gmrId, new Gmr { Id = gmrId }, null, CancellationToken.None);

        result = await client.GetGmr(gmrId, CancellationToken.None);
        result.Should().NotBeNull();
    }

    [Fact]
    public async Task WhenExists_ShouldUpdate()
    {
        var client = CreateDataApiClient();
        var gmrId = Guid.NewGuid().ToString("N");

        var result = await client.GetGmr(gmrId, CancellationToken.None);
        result.Should().BeNull();

        await client.PutGmr(gmrId, new Gmr { Id = gmrId, State = State.Open }, null, CancellationToken.None);

        result = await client.GetGmr(gmrId, CancellationToken.None);
        result.Should().NotBeNull();
        result.Gmr.State.Should().Be(State.Open);
        result.Created.Should().BeAfter(DateTime.MinValue);
        result.Updated.Should().BeAfter(DateTime.MinValue);

        await client.PutGmr(
            gmrId,
            new Gmr { Id = gmrId, State = State.Completed },
            result.ETag,
            CancellationToken.None
        );

        var result2 = await client.GetGmr(gmrId, CancellationToken.None);
        result2.Should().NotBeNull();
        result2.Gmr.State.Should().Be(State.Completed);
        result2.Created.Should().Be(result.Created);
        result2.Updated.Should().BeAfter(result.Updated);
    }

    [Fact]
    public async Task WhenLocalDateTime_ShouldBePreserved()
    {
        var client = CreateDataApiClient();
        var gmrId = Guid.NewGuid().ToString("N");

        // Given an Unspecified date time i.e. local, we expect the value to be stored and
        // not altered when saved to Mongo. See DomainClassMapConfiguration and JSON converter
        // UnknownTimeZoneDateTimeJsonConverter on Domain types where used.

        // The example below is one of several usages and captures the behaviour, not all usages
        var dateTime = new DateTime(2025, 4, 11, 14, 17, 0, DateTimeKind.Unspecified);
        await client.PutGmr(
            gmrId,
            new Gmr { PlannedCrossing = new PlannedCrossing { DepartsAt = dateTime } },
            etag: null,
            CancellationToken.None
        );

        var result = await client.GetGmr(gmrId, CancellationToken.None);
        result.Should().NotBeNull();
        result.Gmr.PlannedCrossing?.DepartsAt.Should().Be(dateTime);
        result.Gmr.PlannedCrossing?.DepartsAt!.Value.Kind.Should().Be(dateTime.Kind);
    }
}
