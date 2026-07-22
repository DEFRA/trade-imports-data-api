using System.Net;
using System.Text.Json;
using AutoFixture;
using Defra.TradeImportsDataApi.Api.Services;
using Defra.TradeImportsDataApi.Data.Entities;
using Defra.TradeImportsDataApi.Domain.Ipaffs;
using Defra.TradeImportsDataApi.Testing;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Trade.Gateway.Api.Contract.Certificate;
using WireMock.Server;
using Xunit.Abstractions;

namespace Defra.TradeImportsDataApi.Api.Tests.Endpoints.TracesCheds;

public class GetTests : EndpointTestBase, IClassFixture<WireMockContext>
{
    private ITracesChedService MockTracesChedService { get; } = Substitute.For<ITracesChedService>();
    private WireMockServer WireMock { get; }
    private const string ChedId = "chedId";
    private readonly VerifySettings _settings;

    public GetTests(ApiWebApplicationFactory factory, ITestOutputHelper outputHelper, WireMockContext context)
        : base(factory, outputHelper)
    {
        WireMock = context.Server;
        WireMock.Reset();

        _settings = new VerifySettings();
        _settings.ScrubMember("traceId");
        _settings.DontScrubDateTimes();
        _settings.DontScrubGuids();
        _settings.DontIgnoreEmptyCollections();
    }

    protected override void ConfigureTestServices(IServiceCollection services)
    {
        base.ConfigureTestServices(services);

        services.AddTransient<ITracesChedService>(_ => MockTracesChedService);
    }

    [Fact]
    public async Task Get_WhenNotFound_ShouldNotBeFound()
    {
        var client = CreateClient();

        var response = await client.GetAsync(TradeImportsDataApi.Testing.Endpoints.TracesCheds.Get(ChedId));

        await VerifyJson(await response.Content.ReadAsStringAsync(), _settings);
    }

    [Fact]
    public async Task Get_WhenFound_ShouldReturnContent()
    {
        var client = CreateClient();
        MockTracesChedService
            .Get(ChedId, Arg.Any<CancellationToken>())
            .Returns(
                new TracesChedEntity
                {
                    Id = ChedId,
                    Ched = new DefraUNVTDCHEDProfile
                    {
                        ExchangedDocument = new ExchangedDocument() { Identifier = "Test" },
                        SpecifiedConsignment = new Consignment(),
                    },
                    Created = new DateTime(2025, 4, 3, 10, 0, 0, DateTimeKind.Utc),
                    Updated = new DateTime(2025, 4, 3, 10, 15, 0, DateTimeKind.Utc),
                    ETag = "etag",
                }
            );

        var response = await client.GetAsync(TradeImportsDataApi.Testing.Endpoints.TracesCheds.Get(ChedId));

        await VerifyJson(await response.Content.ReadAsStringAsync(), _settings)
            .UseMethodName(nameof(Get_WhenFound_ShouldReturnContent));
        await Verify(response, _settings).UseMethodName($"{nameof(Get_WhenFound_ShouldReturnContent)}_response");
    }

    [Fact]
    public async Task Get_WhenUnauthorized_ShouldBeUnauthorized()
    {
        var client = CreateClient(addDefaultAuthorizationHeader: false);

        var response = await client.GetAsync(TradeImportsDataApi.Testing.Endpoints.TracesCheds.Get(ChedId));

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Get_WhenWriteOnly_ShouldBeForbidden()
    {
        var client = CreateClient(testUser: TestUser.WriteOnly);

        var response = await client.GetAsync(TradeImportsDataApi.Testing.Endpoints.TracesCheds.Get(ChedId));

        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
}
