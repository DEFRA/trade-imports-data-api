using System.Net;
using Defra.TradeImportsDataApi.Api.Services;
using Defra.TradeImportsDataApi.Data.Entities;
using Defra.TradeImportsDataApi.Domain.Ipaffs;
using Defra.TradeImportsDataApi.Testing;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using WireMock.Server;
using Xunit.Abstractions;

namespace Defra.TradeImportsDataApi.Api.Tests.Endpoints.CustomsDeclarations;

public class GetImportPreNotificationsByMrnTests : EndpointTestBase, IClassFixture<WireMockContext>
{
    private IImportPreNotificationService MockImportPreNotificationService { get; } =
        Substitute.For<IImportPreNotificationService>();
    private ICustomsDeclarationService MockCustomsDeclarationService { get; } =
        Substitute.For<ICustomsDeclarationService>();
    private WireMockServer WireMock { get; }
    private const string Mrn = "mrn";
    private readonly VerifySettings _settings;

    public GetImportPreNotificationsByMrnTests(
        ApiWebApplicationFactory factory,
        ITestOutputHelper outputHelper,
        WireMockContext context
    )
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

        services.AddTransient<IImportPreNotificationService>(_ => MockImportPreNotificationService);
        services.AddTransient<ICustomsDeclarationService>(_ => MockCustomsDeclarationService);
    }

    [Fact]
    public async Task Get_WhenNotFound_ShouldReturnAnEmptyArray()
    {
        var client = CreateClient();
        MockImportPreNotificationService
            .GetImportPreNotificationsByMrn(Mrn, Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(new List<ImportPreNotificationEntity>()));

        var response = await client.GetAsync(
            TradeImportsDataApi.Testing.Endpoints.CustomsDeclarations.GetImportPreNotifications(Mrn)
        );

        await VerifyJson(await response.Content.ReadAsStringAsync(), _settings);
    }

    [Fact]
    public async Task Get_WhenFound_ShouldReturnContent()
    {
        var client = CreateClient();
        MockImportPreNotificationService
            .GetImportPreNotificationsByMrn(Mrn, Arg.Any<CancellationToken>())
            .Returns(
                [
                    new ImportPreNotificationEntity
                    {
                        Id = "123",
                        ImportPreNotification = new ImportPreNotification(),
                        Created = new DateTime(2025, 4, 3, 10, 0, 0, DateTimeKind.Utc),
                        Updated = new DateTime(2025, 4, 3, 10, 15, 0, DateTimeKind.Utc),
                        ETag = "etag",
                    },
                ]
            );

        var response = await client.GetAsync(
            TradeImportsDataApi.Testing.Endpoints.CustomsDeclarations.GetImportPreNotifications(Mrn)
        );

        await VerifyJson(await response.Content.ReadAsStringAsync(), _settings)
            .UseMethodName(nameof(Get_WhenFound_ShouldReturnContent));
        await Verify(response, _settings).UseMethodName($"{nameof(Get_WhenFound_ShouldReturnContent)}_response");
    }

    [Fact]
    public async Task Get_WhenUnauthorized_ShouldBeUnauthorized()
    {
        var client = CreateClient(addDefaultAuthorizationHeader: false);

        var response = await client.GetAsync(
            TradeImportsDataApi.Testing.Endpoints.CustomsDeclarations.GetImportPreNotifications(Mrn)
        );

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Get_WhenWriteOnly_ShouldBeForbidden()
    {
        var client = CreateClient(testUser: TestUser.WriteOnly);

        var response = await client.GetAsync(
            TradeImportsDataApi.Testing.Endpoints.CustomsDeclarations.GetImportPreNotifications(Mrn)
        );

        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
}
