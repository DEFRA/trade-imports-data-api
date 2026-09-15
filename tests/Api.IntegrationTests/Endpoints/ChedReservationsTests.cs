using Defra.TradeImportsDataApi.Domain.Traces;
using Defra.TradeImportsDataApi.Testing;
using FluentAssertions;
using Xunit.Abstractions;

namespace Defra.TradeImportsDataApi.Api.IntegrationTests.Endpoints;

public class ChedReservationsTests(ITestOutputHelper testOutputHelper) : SqsTestBase(testOutputHelper)
{
    private static Reservation CreateReservation(string chedId, string mrn, string status = "Reserved") =>
        new()
        {
            ChedId = chedId,
            Mrn = mrn,
            Status = status,
            Timestamp = DateTime.UtcNow,
            Commodities =
            [
                new ReservationCommodity
                {
                    GoodsItemNumber = 1,
                    CommodityCode = "123",
                    CertificateLineNumber = 1,
                    UnitOfMeasure = "KGM",
                    Quantity = 100,
                },
            ],
        };

    [Fact]
    public async Task WhenDoesNotExist_ShouldCreateAndRead()
    {
        var chedRef = ImportPreNotificationIdGenerator.Generate();
        var mrn = Guid.NewGuid().ToString("N");
        var client = CreateDataApiClient();
        var httpClient = CreateHttpClient();

        var result = await client.GetChedReservation(chedRef, mrn, CancellationToken.None);
        result.Should().BeNull();

        await client.PutChedReservation(chedRef, mrn, CreateReservation(chedRef, mrn), null, CancellationToken.None);

        result = await client.GetChedReservation(chedRef, mrn, CancellationToken.None);
        result.Should().NotBeNull();

        var resourceId = $"{chedRef}_{mrn}";
        var allResourceEvents = await httpClient.GetFromJsonAsyncSafe<object[]>(
            Testing.Endpoints.ResourceEvents.GetAll(resourceId)
        );
        allResourceEvents.Length.Should().Be(1);
        var unpublishedResourceEvents = await httpClient.GetFromJsonAsyncSafe<object[]>(
            Testing.Endpoints.ResourceEvents.Unpublished(resourceId)
        );
        unpublishedResourceEvents.Length.Should().Be(0);
    }

    [Fact]
    public async Task WhenExists_ShouldUpdate()
    {
        var client = CreateDataApiClient();
        var chedRef = ImportPreNotificationIdGenerator.Generate();
        var mrn = Guid.NewGuid().ToString("N");

        var result = await client.GetChedReservation(chedRef, mrn, CancellationToken.None);
        result.Should().BeNull();

        await client.PutChedReservation(
            chedRef,
            mrn,
            CreateReservation(chedRef, mrn, status: "Reserved"),
            null,
            CancellationToken.None
        );

        result = await client.GetChedReservation(chedRef, mrn, CancellationToken.None);
        result.Should().NotBeNull();
        result.Reservation.Status.Should().Be("Reserved");
        result.Created.Should().BeAfter(DateTime.MinValue);
        result.Updated.Should().BeAfter(DateTime.MinValue);

        await client.PutChedReservation(
            chedRef,
            mrn,
            CreateReservation(chedRef, mrn, status: "Cancelled"),
            result.ETag,
            CancellationToken.None
        );

        var result2 = await client.GetChedReservation(chedRef, mrn, CancellationToken.None);
        result2.Should().NotBeNull();
        result2.Reservation.Status.Should().Be("Cancelled");
        result2.Created.Should().Be(result.Created);
        result2.Updated.Should().BeAfter(result.Updated);
    }

    [Fact]
    public async Task WhenExists_ShouldDelete()
    {
        var client = CreateDataApiClient();
        var chedRef = ImportPreNotificationIdGenerator.Generate();
        var mrn = Guid.NewGuid().ToString("N");

        await client.PutChedReservation(chedRef, mrn, CreateReservation(chedRef, mrn), null, CancellationToken.None);

        var result = await client.GetChedReservation(chedRef, mrn, CancellationToken.None);
        result.Should().NotBeNull();

        await client.DeleteChedReservation(chedRef, mrn, CreateReservation(chedRef, mrn), CancellationToken.None);

        result = await client.GetChedReservation(chedRef, mrn, CancellationToken.None);
        result.Should().BeNull();
    }
}
