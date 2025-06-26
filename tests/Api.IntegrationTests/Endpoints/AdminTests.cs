using System.Net.Http.Json;
using Defra.TradeImportsDataApi.Api.Endpoints.Admin;
using Defra.TradeImportsDataApi.Data.Entities;
using Defra.TradeImportsDataApi.Domain.Ipaffs;
using FluentAssertions;
using MongoDB.Driver;

namespace Defra.TradeImportsDataApi.Api.IntegrationTests.Endpoints;

public class AdminTests : IntegrationTestBase
{
    [Fact]
    public async Task WhenDataExists_ShouldReturn()
    {
        var notifications = GetMongoCollection<ImportPreNotificationEntity>();
        await notifications.DeleteManyAsync(FilterDefinition<ImportPreNotificationEntity>.Empty);

        var client = CreateDataApiClient();
        const string chedRef1 = "CHEDA.GB.2024.1234567";

        await client.PutImportPreNotification(
            chedRef1,
            new ImportPreNotification { ReferenceNumber = chedRef1, Version = 1 },
            null,
            CancellationToken.None
        );
        await Task.Delay(1);
        const string chedRef2 = "CHEDA.GB.2024.1234568";
        await client.PutImportPreNotification(
            chedRef2,
            new ImportPreNotification { ReferenceNumber = chedRef2, Version = 1 },
            null,
            CancellationToken.None
        );

        var httpClient = CreateHttpClient();
        var dto = await httpClient.GetFromJsonAsync<MaxIdResponse>(Testing.Endpoints.Admin.MaxId);

        dto.Should().NotBeNull();
        dto.ImportPreNotification.Should().Be(chedRef2);
    }
}
