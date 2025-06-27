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

        var response = await client.GetAsync(TradeImportsDataApi.Testing.Endpoints.Admin.MaxId);

        await VerifyJson(await response.Content.ReadAsStringAsync(), _settings);
    }
}
