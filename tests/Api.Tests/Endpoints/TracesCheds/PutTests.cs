using System.Net;
using System.Net.Http.Json;
using Defra.TradeImportsDataApi.Api.Exceptions;
using Defra.TradeImportsDataApi.Api.Services;
using Defra.TradeImportsDataApi.Data;
using Defra.TradeImportsDataApi.Data.Entities;
using Defra.TradeImportsDataApi.Domain.Ipaffs;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Trade.Gateway.Api.Contract.Certificate;
using Xunit.Abstractions;

namespace Defra.TradeImportsDataApi.Api.Tests.Endpoints.TracesCheds;

public class PutTests(ApiWebApplicationFactory factory, ITestOutputHelper outputHelper)
    : EndpointTestBase(factory, outputHelper)
{
    private const string ChedId = "chedId";
    private ITracesChedService MockTracesChedService { get; } = Substitute.For<ITracesChedService>();

    protected override void ConfigureTestServices(IServiceCollection services)
    {
        base.ConfigureTestServices(services);

        services.AddTransient<ITracesChedService>(_ => MockTracesChedService);
    }

    [Fact]
    public async Task Put_WhenUnauthorized_ShouldBeUnauthorized()
    {
        var client = CreateClient(addDefaultAuthorizationHeader: false);

        var response = await client.PutAsJsonAsync(
            Testing.Endpoints.TracesCheds.Put(ChedId),
            new ImportPreNotification()
        );

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Put_WhenReadOnly_ShouldBeForbidden()
    {
        var client = CreateClient(testUser: TestUser.ReadOnly);

        var response = await client.PutAsJsonAsync(
            Testing.Endpoints.TracesCheds.Put(ChedId),
            new ImportPreNotification()
        );

        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task Put_WhenEntityNotFound_ShouldBeNotFound()
    {
        var client = CreateClient();
        MockTracesChedService
            .Insert(Arg.Any<TracesChedEntity>(), Arg.Any<CancellationToken>())
            .Throws(new EntityNotFoundException("entityType", "entityId"));

        var response = await client.PutAsJsonAsync(
            Testing.Endpoints.TracesCheds.Put(ChedId),
            new DefraUNVTDCHEDProfile
            {
                ExchangedDocument = new ExchangedDocument() { Identifier = "Test" },
                SpecifiedConsignment = new Consignment(),
            }
        );

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Put_WhenConcurrencyException_ShouldBeConflict()
    {
        var client = CreateClient();
        MockTracesChedService
            .Insert(Arg.Any<TracesChedEntity>(), Arg.Any<CancellationToken>())
            .Throws(new ConcurrencyException("entityId", "etag"));

        var response = await client.PutAsJsonAsync(
            Testing.Endpoints.TracesCheds.Put(ChedId),
            new DefraUNVTDCHEDProfile
            {
                ExchangedDocument = new ExchangedDocument() { Identifier = "Test" },
                SpecifiedConsignment = new Consignment(),
            }
        );

        response.StatusCode.Should().Be(HttpStatusCode.Conflict);
    }
}
