using System.Net.Http.Json;
using Defra.TradeImportsDataApi.Api.Endpoints.Admin;
using Defra.TradeImportsDataApi.Domain.CustomsDeclaration;
using Defra.TradeImportsDataApi.Domain.Gvms;
using Defra.TradeImportsDataApi.Domain.Ipaffs;
using Defra.TradeImportsDataApi.Testing;
using FluentAssertions;

namespace Defra.TradeImportsDataApi.Api.IntegrationTests.Endpoints;

public class AdminTests : IntegrationTestBase
{
    [Fact]
    public async Task WhenDataExists_ShouldReturn()
    {
        var client = CreateDataApiClient();
        var chedRef1 = ImportPreNotificationIdGenerator.Generate();

        await client.PutImportPreNotification(
            chedRef1,
            new ImportPreNotification { ReferenceNumber = chedRef1, Version = 1 },
            null,
            CancellationToken.None
        );
        await Task.Delay(1);
        var chedRef2 = ImportPreNotificationIdGenerator.Generate();
        await client.PutImportPreNotification(
            chedRef2,
            new ImportPreNotification { ReferenceNumber = chedRef2, Version = 1 },
            null,
            CancellationToken.None
        );

        var mrn1 = Guid.NewGuid().ToString();
        await client.PutCustomsDeclaration(mrn1, new CustomsDeclaration(), null, CancellationToken.None);
        await Task.Delay(1);
        var mrn2 = Guid.NewGuid().ToString();
        await client.PutCustomsDeclaration(mrn2, new CustomsDeclaration(), null, CancellationToken.None);

        var gmr1 = Guid.NewGuid().ToString();
        await client.PutGmr(gmr1, new Gmr(), null, CancellationToken.None);
        await Task.Delay(1);
        var gmr2 = Guid.NewGuid().ToString();
        await client.PutGmr(gmr2, new Gmr(), null, CancellationToken.None);

        await client.PutProcessingError(mrn1, [], null, CancellationToken.None);
        await Task.Delay(1);
        await client.PutProcessingError(mrn2, [], null, CancellationToken.None);

        var httpClient = CreateHttpClient();
        var dto = await httpClient.GetFromJsonAsync<LatestResponse>(Testing.Endpoints.Admin.Latest);

        dto.Should().NotBeNull();
        dto.ImportPreNotification.Should().Be(chedRef2);
        dto.CustomsDeclaration.Should().Be(mrn2);
        dto.Gmr.Should().Be(gmr2);
        dto.ProcessingError.Should().Be(mrn2);
    }
}
