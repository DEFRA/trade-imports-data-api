using System.Net;
using Defra.TradeImportsDataApi.Api.Exceptions;
using Defra.TradeImportsDataApi.Api.Services;
using Defra.TradeImportsDataApi.Data;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Xunit.Abstractions;

namespace Defra.TradeImportsDataApi.Api.Tests.Endpoints.ChedReservations;

public class DeleteTests(ApiWebApplicationFactory factory, ITestOutputHelper outputHelper)
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

    [Fact]
    public async Task Delete_WhenUnauthorized_ShouldBeUnauthorized()
    {
        var client = CreateClient(addDefaultAuthorizationHeader: false);

        var response = await client.DeleteAsync(Testing.Endpoints.ChedReservations.Delete(ChedId, Mrn));

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Delete_WhenReadOnly_ShouldBeForbidden()
    {
        var client = CreateClient(testUser: TestUser.ReadOnly);

        var response = await client.DeleteAsync(Testing.Endpoints.ChedReservations.Delete(ChedId, Mrn));

        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task Delete_WhenFound_ShouldReturnNoContent()
    {
        var client = CreateClient();
        MockChedReservationService.Delete(ChedId, Mrn, Arg.Any<CancellationToken>()).Returns(Task.CompletedTask);

        var response = await client.DeleteAsync(Testing.Endpoints.ChedReservations.Delete(ChedId, Mrn));

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task Delete_WhenEntityNotFound_ShouldBeNotFound()
    {
        var client = CreateClient();
        MockChedReservationService
            .Delete(ChedId, Mrn, Arg.Any<CancellationToken>())
            .Throws(new EntityNotFoundException("entityType", "entityId"));

        var response = await client.DeleteAsync(Testing.Endpoints.ChedReservations.Delete(ChedId, Mrn));

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Delete_WhenConcurrencyException_ShouldBeConflict()
    {
        var client = CreateClient();
        MockChedReservationService
            .Delete(ChedId, Mrn, Arg.Any<CancellationToken>())
            .Throws(new ConcurrencyException("entityId", "etag"));

        var response = await client.DeleteAsync(Testing.Endpoints.ChedReservations.Delete(ChedId, Mrn));

        response.StatusCode.Should().Be(HttpStatusCode.Conflict);
    }
}
