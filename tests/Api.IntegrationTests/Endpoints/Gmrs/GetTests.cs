using System.Net;
using Defra.TradeImportsDataApi.Api.Services;
using Defra.TradeImportsDataApi.Data.Entities;
using Defra.TradeImportsDataApi.Domain.Gvms;
using Defra.TradeImportsDataApi.Testing;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using WireMock.Server;
using Xunit.Abstractions;

namespace Defra.TradeImportsDataApi.Api.IntegrationTests.Endpoints.Gmrs;

public class GetTests : EndpointTestBase, IClassFixture<WireMockContext>
{
    private IGmrService MockGmrService { get; } = Substitute.For<IGmrService>();
    private WireMockServer WireMock { get; }
    private const string GmrId = "gmrId";
    private readonly VerifySettings _settings;

    public GetTests(ApiWebApplicationFactory factory, ITestOutputHelper outputHelper, WireMockContext context)
        : base(factory, outputHelper)
    {
        WireMock = context.Server;
        WireMock.Reset();

        _settings = new VerifySettings();
        _settings.ScrubMember("traceId");
        _settings.DontScrubDateTimes();
    }

    protected override void ConfigureTestServices(IServiceCollection services)
    {
        base.ConfigureTestServices(services);

        services.AddTransient<IGmrService>(_ => MockGmrService);
    }

    [Fact]
    public async Task Get_WhenNotFound_ShouldNotBeFound()
    {
        var client = CreateClient();

        var response = await client.GetAsync(TradeImportsDataApi.Testing.Endpoints.Gmrs.Get(GmrId));

        await VerifyJson(await response.Content.ReadAsStringAsync(), _settings);
    }

    [Fact]
    public async Task Get_WhenException_ShouldBeInternalServerError()
    {
        var client = CreateClient();
        MockGmrService.GetGmr(GmrId, Arg.Any<CancellationToken>()).Throws(new Exception("BOOM!"));

        var response = await client.GetAsync(TradeImportsDataApi.Testing.Endpoints.Gmrs.Get(GmrId));

        await VerifyJson(await response.Content.ReadAsStringAsync(), _settings);
    }

    [Fact]
    public async Task Get_WhenBadRequest_ShouldBeBadRequest()
    {
        var client = CreateClient();
        MockGmrService
            .GetGmr(GmrId, Arg.Any<CancellationToken>())
            .Throws(new BadHttpRequestException("Bad Request Detail"));

        var response = await client.GetAsync(TradeImportsDataApi.Testing.Endpoints.Gmrs.Get(GmrId));

        await VerifyJson(await response.Content.ReadAsStringAsync(), _settings);
    }

    [Fact]
    public async Task Get_WhenFound_ShouldReturnContent()
    {
        var client = CreateClient();
        MockGmrService
            .GetGmr(GmrId, Arg.Any<CancellationToken>())
            .Returns(
                new GmrEntity
                {
                    Id = GmrId,
                    Data = new Gmr(),
                    Created = new DateTime(2025, 4, 3, 10, 0, 0, DateTimeKind.Utc),
                    Updated = new DateTime(2025, 4, 3, 10, 15, 0, DateTimeKind.Utc),
                    ETag = "etag",
                }
            );

        var response = await client.GetAsync(TradeImportsDataApi.Testing.Endpoints.Gmrs.Get(GmrId));

        await VerifyJson(await response.Content.ReadAsStringAsync(), _settings)
            .UseMethodName(nameof(Get_WhenFound_ShouldReturnContent));
        await Verify(response, _settings).UseMethodName($"{nameof(Get_WhenFound_ShouldReturnContent)}_response");
    }

    [Fact]
    public async Task Get_WhenUnauthorized_ShouldBeUnauthorized()
    {
        var client = CreateClient(addDefaultAuthorizationHeader: false);

        var response = await client.GetAsync(TradeImportsDataApi.Testing.Endpoints.Gmrs.Get(GmrId));

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Get_WhenWriteOnly_ShouldBeForbidden()
    {
        var client = CreateClient(testUser: TestUser.WriteOnly);

        var response = await client.GetAsync(TradeImportsDataApi.Testing.Endpoints.Gmrs.Get(GmrId));

        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
}
