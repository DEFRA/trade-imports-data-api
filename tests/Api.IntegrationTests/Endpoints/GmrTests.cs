using Defra.TradeImportsDataApi.Api.Client;
using Defra.TradeImportsDataApi.Domain.Gvms;
using FluentAssertions;

namespace Defra.TradeImportsDataApi.Api.IntegrationTests.Endpoints;

public class GmrTests : IntegrationTestBase
{
    [Fact]
    public async Task WhenDoesNotExist_ShouldCreateAndRead()
    {
        var client = new TradeImportsDataApiClient(new HttpClient { BaseAddress = new Uri("http://localhost:8080") });
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
        var client = new TradeImportsDataApiClient(new HttpClient { BaseAddress = new Uri("http://localhost:8080") });
        var gmrId = Guid.NewGuid().ToString("N");

        var result = await client.GetGmr(gmrId, CancellationToken.None);
        result.Should().BeNull();

        await client.PutGmr(gmrId, new Gmr { Id = gmrId, State = State.Open }, null, CancellationToken.None);

        result = await client.GetGmr(gmrId, CancellationToken.None);
        result.Should().NotBeNull();
        result.Gmr.State.Should().Be(State.Open);

        await client.PutGmr(
            gmrId,
            new Gmr { Id = gmrId, State = State.Finalised },
            result.ETag,
            CancellationToken.None
        );

        result = await client.GetGmr(gmrId, CancellationToken.None);
        result.Should().NotBeNull();
        result.Gmr.State.Should().Be(State.Finalised);
    }
}
