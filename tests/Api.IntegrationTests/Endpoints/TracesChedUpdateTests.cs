using Defra.TradeImportsDataApi.Api.Client;
using Defra.TradeImportsDataApi.Data.Entities;
using Defra.TradeImportsDataApi.Testing;
using FluentAssertions;
using MongoDB.Driver;
using Trade.Gateway.Api.Contract.Certificate;

namespace Defra.TradeImportsDataApi.Api.IntegrationTests.Endpoints;

public class TracesChedUpdateTests : IntegrationTestBase, IAsyncLifetime
{
    public required IMongoCollection<TracesChedEntity> TracesCheds { get; set; }
    public required TradeImportsDataApiClient DataApiClient { get; set; }

    private DateTime _from;

    public async Task InitializeAsync()
    {
        TracesCheds = GetMongoCollection<TracesChedEntity>();

        await TracesCheds.DeleteManyAsync(FilterDefinition<TracesChedEntity>.Empty);

        DataApiClient = CreateDataApiClient();

        // Records are stamped with UtcNow on write, so anchor the window just before the test writes anything
        _from = DateTime.UtcNow.AddSeconds(-1);
    }

    public Task DisposeAsync() => Task.CompletedTask;

    [Fact]
    public async Task WhenChedCreated_ThenChedRefAndUpdatedAsExpected()
    {
        var chedRef = ImportPreNotificationIdGenerator.Generate();

        await CreateChed(chedRef);

        var ched = await DataApiClient.GetTracesChed(chedRef, CancellationToken.None);

        var result = await GetUpdates();

        result.Total.Should().Be(1);
        result.TracesChedUpdates.Should().HaveCount(1);
        result.TracesChedUpdates[0].ReferenceNumber.Should().Be(chedRef);
        result.TracesChedUpdates[0].Updated.Should().Be(ched!.Updated);
    }

    [Fact]
    public async Task WhenChedUpdated_ThenLatestUpdatedReturnedOnce()
    {
        var chedRef = ImportPreNotificationIdGenerator.Generate();

        await CreateChed(chedRef);
        var created = await DataApiClient.GetTracesChed(chedRef, CancellationToken.None);

        await Task.Delay(1);
        await UpdateChed(chedRef, created!.ETag);
        var updated = await DataApiClient.GetTracesChed(chedRef, CancellationToken.None);

        var result = await GetUpdates();

        // The CHED reference is the document id, so an update replaces rather than adds a row
        result.Total.Should().Be(1);
        result.TracesChedUpdates.Should().HaveCount(1);
        result.TracesChedUpdates[0].ReferenceNumber.Should().Be(chedRef);
        result.TracesChedUpdates[0].Updated.Should().Be(updated!.Updated);
        updated.Updated.Should().BeAfter(created.Updated);
    }

    [Fact]
    public async Task WhenChedUpdatedOutsideOfWindow_ThenNotReturned()
    {
        var chedRef = ImportPreNotificationIdGenerator.Generate();

        await CreateChed(chedRef);

        var result = await GetUpdates(from: _from.AddMinutes(-30), to: _from.AddMinutes(-29));

        result.Total.Should().Be(0);
        result.TracesChedUpdates.Should().BeEmpty();
    }

    [Fact]
    public async Task WhenPaging_ThenPagesReturnedAsExpected()
    {
        const int total = 12;
        const int pageSize = 5;

        for (var i = 1; i <= total; i++)
        {
            await CreateChed($"CHEDA.GB.2025.{i.ToString().PadLeft(7, '0')}");
            await Task.Delay(1);
        }

        for (var page = 1; page <= 3; page++)
        {
            var result = await GetUpdates(page: page, pageSize: pageSize);

            result.Total.Should().Be(total);
            result.Page.Should().Be(page);
            result.PageSize.Should().Be(pageSize);

            var lastPage = page == 3;
            result.TracesChedUpdates.Should().HaveCount(lastPage ? 2 : pageSize);

            var pageOffset = (page - 1) * pageSize;

            for (var i = 1; i <= result.TracesChedUpdates.Count; i++)
            {
                result
                    .TracesChedUpdates[i - 1]
                    .ReferenceNumber.Should()
                    .Be($"CHEDA.GB.2025.{(i + pageOffset).ToString().PadLeft(7, '0')}");
            }
        }
    }

    private async Task CreateChed(string chedRef) =>
        await DataApiClient.PutTracesChed(
            chedRef,
            new DefraUNVTDCHEDProfile
            {
                ExchangedDocument = new ExchangedDocument { Identifier = chedRef },
                SpecifiedConsignment = new Consignment(),
                Model = "Test",
            },
            null,
            CancellationToken.None
        );

    private async Task UpdateChed(string chedRef, string? etag) =>
        await DataApiClient.PutTracesChed(
            chedRef,
            new DefraUNVTDCHEDProfile
            {
                ExchangedDocument = new ExchangedDocument { Identifier = chedRef },
                SpecifiedConsignment = new Consignment(),
                Model = "Test1",
            },
            etag,
            CancellationToken.None
        );

    private async Task<TracesChedUpdatesResponse> GetUpdates(
        DateTime? from = null,
        DateTime? to = null,
        int? page = null,
        int? pageSize = null
    ) =>
        await DataApiClient.GetTracesChedUpdates(
            new TracesChedUpdatesRequest
            {
                From = from ?? _from,
                To = to ?? _from.AddHours(1),
                Page = page,
                PageSize = pageSize,
            },
            CancellationToken.None
        );
}
