using System.Net.Http.Json;
using Defra.TradeImportsDataApi.Api.Client;
using Defra.TradeImportsDataApi.Api.Endpoints.Admin;
using Defra.TradeImportsDataApi.Data.Entities;
using Defra.TradeImportsDataApi.Domain.Ipaffs;
using FluentAssertions;
using MongoDB.Driver;
using Trade.Gateway.Api.Contract.Certificate;

namespace Defra.TradeImportsDataApi.Api.IntegrationTests.Endpoints;

public class AdminTests : IntegrationTestBase
{
    [Fact]
    public async Task WhenDataExists_ShouldReturn()
    {
        var notifications = GetMongoCollection<ImportPreNotificationEntity>();
        await notifications.DeleteManyAsync(FilterDefinition<ImportPreNotificationEntity>.Empty);
        var tracesCheds = GetMongoCollection<TracesChedEntity>();
        await tracesCheds.DeleteManyAsync(FilterDefinition<TracesChedEntity>.Empty);

        var client = CreateDataApiClient();
        await CreateNotification(client, 1);
        await CreateNotification(client, 2);
        await CreateNotification(client, 10);
        await CreateNotification(client, 3);
        await CreateNotification(client, 8);
        await CreateNotification(client, 21);

        var httpClient = CreateHttpClient();
        var dto = await httpClient.GetFromJsonAsync<MaxIdResponse>(Testing.Endpoints.Admin.MaxId);

        dto.Should().NotBeNull();
        dto.ImportPreNotification.Should().Be("CHEDA.GB.2024.0000021");
    }

    [Fact]
    public async Task WhenTracesChedHasHigherId_ShouldReturnTracesChed()
    {
        var notifications = GetMongoCollection<ImportPreNotificationEntity>();
        await notifications.DeleteManyAsync(FilterDefinition<ImportPreNotificationEntity>.Empty);
        var tracesCheds = GetMongoCollection<TracesChedEntity>();
        await tracesCheds.DeleteManyAsync(FilterDefinition<TracesChedEntity>.Empty);

        var client = CreateDataApiClient();
        await CreateNotification(client, 21);
        await CreateTracesChed(client, 99);

        var httpClient = CreateHttpClient();
        var dto = await httpClient.GetFromJsonAsync<MaxIdResponse>(Testing.Endpoints.Admin.MaxId);

        dto.Should().NotBeNull();
        dto.ImportPreNotification.Should().Be("CHEDA.GB.2024.0000099");
    }

    private static async Task CreateNotification(TradeImportsDataApiClient client, int id)
    {
        var chedRef = $"CHEDA.GB.2024.{id.ToString().PadLeft(7, '0')}";

        await client.PutImportPreNotification(
            chedRef,
            new ImportPreNotification { ReferenceNumber = chedRef, Version = 1 },
            null,
            CancellationToken.None
        );
    }

    private static async Task CreateTracesChed(TradeImportsDataApiClient client, int id)
    {
        var chedRef = $"CHEDA.GB.2024.{id.ToString().PadLeft(7, '0')}";

        await client.PutTracesChed(
            chedRef,
            new DefraUNVTDCHEDProfile
            {
                ExchangedDocument = new ExchangedDocument { Identifier = chedRef },
                SpecifiedConsignment = new Consignment(),
            },
            null,
            CancellationToken.None
        );
    }
}
