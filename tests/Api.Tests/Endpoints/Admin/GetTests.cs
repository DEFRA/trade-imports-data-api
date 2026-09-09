using System.Net;
using Defra.TradeImportsDataApi.Api.Data;
using Defra.TradeImportsDataApi.Testing;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using WireMock.Server;
using Xunit.Abstractions;

namespace Defra.TradeImportsDataApi.Api.Tests.Endpoints.Admin;

public class GetTests : EndpointTestBase, IClassFixture<WireMockContext>
{
    private WireMockServer WireMock { get; }
    private readonly VerifySettings _settings;

    private IImportPreNotificationRepository MockImportPreNotificationRepository { get; } =
        Substitute.For<IImportPreNotificationRepository>();

    private ITracesChedRepository MockTracesChedRepository { get; } = Substitute.For<ITracesChedRepository>();

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

        services.AddTransient<IImportPreNotificationRepository>(_ => MockImportPreNotificationRepository);
        services.AddTransient<ITracesChedRepository>(_ => MockTracesChedRepository);
    }

    [Fact]
    public async Task Get_WhenUnauthorized_ShouldBeUnauthorized()
    {
        var client = CreateClient(addDefaultAuthorizationHeader: false);

        var response = await client.GetAsync(TradeImportsDataApi.Testing.Endpoints.Admin.MaxId);

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Get_WhenWriteOnly_ShouldBeForbidden()
    {
        var client = CreateClient(testUser: TestUser.WriteOnly);

        var response = await client.GetAsync(TradeImportsDataApi.Testing.Endpoints.Admin.MaxId);

        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task Get_WhenAuthorized_MaxId_ShouldBeOk()
    {
        var client = CreateClient();
        MockImportPreNotificationRepository.GetMaxId(Arg.Any<CancellationToken>()).Returns("CHEDA.GB.2024.1234567");
        MockTracesChedRepository.GetMaxId(Arg.Any<CancellationToken>()).Returns("CHEDD.GB.2024.7654321");

        var response = await client.GetAsync(TradeImportsDataApi.Testing.Endpoints.Admin.MaxId);

        await VerifyJson(await response.Content.ReadAsStringAsync(), _settings);
    }

    [Fact]
    public async Task Get_WhenBothImportPreNotificationAndTracesChedAreNull_ShouldReturnNull()
    {
        var client = CreateClient();

        var response = await client.GetAsync(TradeImportsDataApi.Testing.Endpoints.Admin.MaxId);

        await VerifyJson(await response.Content.ReadAsStringAsync(), _settings);
    }

    [Fact]
    public async Task Get_WhenTracesChedHasHigherId_ShouldReturnTracesChed()
    {
        var client = CreateClient();
        MockImportPreNotificationRepository.GetMaxId(Arg.Any<CancellationToken>()).Returns("CHEDA.GB.2024.7654321");
        MockTracesChedRepository.GetMaxId(Arg.Any<CancellationToken>()).Returns("CHEDD.GB.2024.9999999");

        var response = await client.GetAsync(TradeImportsDataApi.Testing.Endpoints.Admin.MaxId);

        await VerifyJson(await response.Content.ReadAsStringAsync(), _settings);
    }

    [Fact]
    public async Task Get_WhenTracesChedHasLowerId_ShouldReturnImportPreNotification()
    {
        var client = CreateClient();
        MockImportPreNotificationRepository.GetMaxId(Arg.Any<CancellationToken>()).Returns("CHEDA.GB.2024.9999999");
        MockTracesChedRepository.GetMaxId(Arg.Any<CancellationToken>()).Returns("CHEDD.GB.2024.7654321");

        var response = await client.GetAsync(TradeImportsDataApi.Testing.Endpoints.Admin.MaxId);

        await VerifyJson(await response.Content.ReadAsStringAsync(), _settings);
    }

    [Fact]
    public async Task Get_WhenTracesChedHasHigherIdOnSevenToEightDigitBoundary_ShouldReturnTracesChed()
    {
        var client = CreateClient();
        MockImportPreNotificationRepository.GetMaxId(Arg.Any<CancellationToken>()).Returns("CHEDA.GB.2024.9999999");
        MockTracesChedRepository.GetMaxId(Arg.Any<CancellationToken>()).Returns("CHEDD.GB.2024.10000000");

        var response = await client.GetAsync(TradeImportsDataApi.Testing.Endpoints.Admin.MaxId);

        await VerifyJson(await response.Content.ReadAsStringAsync(), _settings);
    }

    [Fact]
    public async Task Get_WhenTracesChedMissing_ShouldReturnImportPreNotification()
    {
        var client = CreateClient();
        MockImportPreNotificationRepository.GetMaxId(Arg.Any<CancellationToken>()).Returns("CHEDA.GB.2024.1234567");
        MockTracesChedRepository.GetMaxId(Arg.Any<CancellationToken>()).Returns((string?)null);

        var response = await client.GetAsync(TradeImportsDataApi.Testing.Endpoints.Admin.MaxId);

        await VerifyJson(await response.Content.ReadAsStringAsync(), _settings);
    }
}
