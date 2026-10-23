using System.Net;
using Defra.TradeImportsDataApi.Api.Services;
using Defra.TradeImportsDataApi.Data.Entities;
using Defra.TradeImportsDataApi.Domain.Iuu;
using Defra.TradeImportsDataApi.Testing;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using WireMock.Server;
using Xunit.Abstractions;

namespace Defra.TradeImportsDataApi.Api.Tests.Endpoints.IuuNotifications;

public class GetTests : EndpointTestBase, IClassFixture<WireMockContext>
{
    private IIuuNotificationService MockIuuNotificationService { get; } = Substitute.For<IIuuNotificationService>();
    private WireMockServer WireMock { get; }
    private const string IuuReference = "IUU.FR.2026.0000001";
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

        services.AddTransient<IIuuNotificationService>(_ => MockIuuNotificationService);
    }

    [Fact]
    public async Task Get_WhenNotFound_ShouldNotBeFound()
    {
        var client = CreateClient();

        var response = await client.GetAsync(TradeImportsDataApi.Testing.Endpoints.IuuNotifications.Get(IuuReference));

        await VerifyJson(await response.Content.ReadAsStringAsync(), _settings);
    }

    [Fact]
    public async Task Get_WhenFound_ShouldReturnContent()
    {
        var client = CreateClient();
        MockIuuNotificationService
            .Get(IuuReference, Arg.Any<CancellationToken>())
            .Returns(
                new IuuNotificationEntity
                {
                    Id = IuuReference,
                    IuuNotification = new IuuNotification
                    {
                        Reference = IuuReference,
                        LastUpdated = new DateTime(2026, 10, 16, 09, 0, 0, DateTimeKind.Utc),
                        IuuOption = "IUUOK",
                    },
                    Created = new DateTime(2026, 10, 16, 09, 0, 0, DateTimeKind.Utc),
                    Updated = new DateTime(2026, 10, 16, 09, 15, 0, DateTimeKind.Utc),
                    ETag = "etag",
                }
            );

        var response = await client.GetAsync(TradeImportsDataApi.Testing.Endpoints.IuuNotifications.Get(IuuReference));

        await VerifyJson(await response.Content.ReadAsStringAsync(), _settings)
            .UseMethodName(nameof(Get_WhenFound_ShouldReturnContent));
        await Verify(response, _settings).UseMethodName($"{nameof(Get_WhenFound_ShouldReturnContent)}_response");
    }

    [Fact]
    public async Task Get_WhenUnauthorized_ShouldBeUnauthorized()
    {
        var client = CreateClient(addDefaultAuthorizationHeader: false);

        var response = await client.GetAsync(TradeImportsDataApi.Testing.Endpoints.IuuNotifications.Get(IuuReference));

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Get_WhenWriteOnly_ShouldBeForbidden()
    {
        var client = CreateClient(testUser: TestUser.WriteOnly);

        var response = await client.GetAsync(TradeImportsDataApi.Testing.Endpoints.IuuNotifications.Get(IuuReference));

        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
}
