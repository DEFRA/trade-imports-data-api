using System.Net;
using System.Net.Http.Json;
using Defra.TradeImportsDataApi.Api.Exceptions;
using Defra.TradeImportsDataApi.Api.Services;
using Defra.TradeImportsDataApi.Data;
using Defra.TradeImportsDataApi.Data.Entities;
using Defra.TradeImportsDataApi.Domain.Traces;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Xunit.Abstractions;

namespace Defra.TradeImportsDataApi.Api.Tests.Endpoints.ChedReservations;

public class PutTests(ApiWebApplicationFactory factory, ITestOutputHelper outputHelper)
    : EndpointTestBase(factory, outputHelper)
{
    private const string ChedId = "chedId";
    private const string Mrn = "mrn";
    private IChedReservationService MockChedReservationService { get; } = Substitute.For<IChedReservationService>();

    protected override void ConfigureTestServices(IServiceCollection services)
    {
        base.ConfigureTestServices(services);

        services.AddTransient<IChedReservationService>(_ => MockChedReservationService);
    }

    private static Reservation CreateReservation() =>
        new()
        {
            ChedId = ChedId,
            Mrn = Mrn,
            Status = "Reserved",
            Timestamp = new DateTime(2025, 4, 3, 10, 0, 0, DateTimeKind.Utc),
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
    public async Task Put_WhenUnauthorized_ShouldBeUnauthorized()
    {
        var client = CreateClient(addDefaultAuthorizationHeader: false);

        var response = await client.PutAsJsonAsync(
            Testing.Endpoints.ChedReservations.Put(ChedId, Mrn),
            CreateReservation()
        );

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Put_WhenReadOnly_ShouldBeForbidden()
    {
        var client = CreateClient(testUser: TestUser.ReadOnly);

        var response = await client.PutAsJsonAsync(
            Testing.Endpoints.ChedReservations.Put(ChedId, Mrn),
            CreateReservation()
        );

        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task Put_WhenNoEtag_ShouldInsertAndReturnOk()
    {
        var client = CreateClient();
        var entity = new ChedReservationEntity
        {
            Id = $"{ChedId}_{Mrn}",
            Reservation = CreateReservation(),
            Created = new DateTime(2025, 4, 3, 10, 0, 0, DateTimeKind.Utc),
            Updated = new DateTime(2025, 4, 3, 10, 0, 0, DateTimeKind.Utc),
            ETag = "etag",
        };
        MockChedReservationService
            .Upsert(Arg.Any<ChedReservationEntity>(), null, Arg.Any<CancellationToken>())
            .Returns(entity);

        var response = await client.PutAsJsonAsync(
            Testing.Endpoints.ChedReservations.Put(ChedId, Mrn),
            CreateReservation()
        );

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Put_WhenEntityNotFound_ShouldBeNotFound()
    {
        var client = CreateClient();
        MockChedReservationService
            .Upsert(Arg.Any<ChedReservationEntity>(), Arg.Any<string?>(), Arg.Any<CancellationToken>())
            .Throws(new EntityNotFoundException("entityType", "entityId"));

        var response = await client.PutAsJsonAsync(
            Testing.Endpoints.ChedReservations.Put(ChedId, Mrn),
            CreateReservation()
        );

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Put_WhenConcurrencyException_ShouldBeConflict()
    {
        var client = CreateClient();
        MockChedReservationService
            .Upsert(Arg.Any<ChedReservationEntity>(), Arg.Any<string?>(), Arg.Any<CancellationToken>())
            .Throws(new ConcurrencyException("entityId", "etag"));

        var response = await client.PutAsJsonAsync(
            Testing.Endpoints.ChedReservations.Put(ChedId, Mrn),
            CreateReservation()
        );

        response.StatusCode.Should().Be(HttpStatusCode.Conflict);
    }
}
