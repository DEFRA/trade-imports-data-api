using System.Net;
using System.Text.Json;
using AutoFixture;
using Defra.TradeImportsDataApi.Api.Services;
using Defra.TradeImportsDataApi.Data.Entities;
using Defra.TradeImportsDataApi.Domain.Gvms;
using Defra.TradeImportsDataApi.Domain.Ipaffs;
using Defra.TradeImportsDataApi.Testing;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using WireMock.Server;
using Xunit.Abstractions;

namespace Defra.TradeImportsDataApi.Api.Tests.Endpoints.ImportPreNotifications;

public class GetTests : EndpointTestBase, IClassFixture<WireMockContext>
{
    private IImportPreNotificationService MockImportPreNotificationService { get; } =
        Substitute.For<IImportPreNotificationService>();
    private WireMockServer WireMock { get; }
    private const string ChedId = "chedId";
    private readonly VerifySettings _settings;
    private static readonly JsonSerializerOptions s_jsonOptions = new() { WriteIndented = true };

    public GetTests(ApiWebApplicationFactory factory, ITestOutputHelper outputHelper, WireMockContext context)
        : base(factory, outputHelper)
    {
        WireMock = context.Server;
        WireMock.Reset();

        _settings = new VerifySettings();
        _settings.ScrubMember("traceId");
        _settings.DontScrubDateTimes();
        _settings.DontScrubGuids();
    }

    protected override void ConfigureTestServices(IServiceCollection services)
    {
        base.ConfigureTestServices(services);

        services.AddTransient<IImportPreNotificationService>(_ => MockImportPreNotificationService);
    }

    [Fact]
    public async Task Get_WhenNotFound_ShouldNotBeFound()
    {
        var client = CreateClient();

        var response = await client.GetAsync(TradeImportsDataApi.Testing.Endpoints.ImportPreNotifications.Get(ChedId));

        await VerifyJson(await response.Content.ReadAsStringAsync(), _settings);
    }

    [Fact]
    public async Task Get_WhenFound_ShouldReturnContent()
    {
        var client = CreateClient();
        MockImportPreNotificationService
            .GetImportPreNotification(ChedId, Arg.Any<CancellationToken>())
            .Returns(
                new ImportPreNotificationEntity
                {
                    Id = ChedId,
                    CustomsDeclarationIdentifier = ChedId,
                    ImportPreNotification = new ImportPreNotification(),
                    Created = new DateTime(2025, 4, 3, 10, 0, 0, DateTimeKind.Utc),
                    Updated = new DateTime(2025, 4, 3, 10, 15, 0, DateTimeKind.Utc),
                    ETag = "etag",
                }
            );

        var response = await client.GetAsync(TradeImportsDataApi.Testing.Endpoints.ImportPreNotifications.Get(ChedId));

        await VerifyJson(await response.Content.ReadAsStringAsync(), _settings)
            .UseMethodName(nameof(Get_WhenFound_ShouldReturnContent));
        await Verify(response, _settings).UseMethodName($"{nameof(Get_WhenFound_ShouldReturnContent)}_response");
    }

    [Fact]
    public async Task Get_WhenUnauthorized_ShouldBeUnauthorized()
    {
        var client = CreateClient(addDefaultAuthorizationHeader: false);

        var response = await client.GetAsync(TradeImportsDataApi.Testing.Endpoints.ImportPreNotifications.Get(ChedId));

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Get_WhenWriteOnly_ShouldBeForbidden()
    {
        var client = CreateClient(testUser: TestUser.WriteOnly);

        var response = await client.GetAsync(TradeImportsDataApi.Testing.Endpoints.ImportPreNotifications.Get(ChedId));

        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task Get_WhenGenerating_GetTests_DomainExample_ShouldSerialize()
    {
        var fixture = new Fixture();
        fixture.Customize<DateOnly>(o => o.FromFactory((DateTime dt) => DateOnly.FromDateTime(dt)));
        fixture.Customize<TimeOnly>(o => o.FromFactory((DateTime dt) => TimeOnly.FromDateTime(dt)));
        var importPreNotification = fixture.Create<ImportPreNotification>();
        var serialized = JsonSerializer.Serialize(importPreNotification, s_jsonOptions);

        // Take this file and replace GetTests_DomainExample.json when needed
        await File.WriteAllTextAsync(
            $"{nameof(Get_WhenGenerating_GetTests_DomainExample_ShouldSerialize)}_ImportPreNotification.json",
            serialized
        );

        serialized.Should().NotBeNull();
    }

    [Fact]
    public async Task Get_WhenReturningDomainExample_ShouldBeCorrectJson()
    {
        // See test above for generation of new content
        var body = EmbeddedResource.GetBody(GetType(), "GetTests_DomainExample.json");
        var importPreNotification =
            JsonSerializer.Deserialize<ImportPreNotification>(body, s_jsonOptions)
            ?? throw new InvalidOperationException();
        var client = CreateClient();
        MockImportPreNotificationService
            .GetImportPreNotification(ChedId, Arg.Any<CancellationToken>())
            .Returns(
                new ImportPreNotificationEntity
                {
                    Id = ChedId,
                    ImportPreNotification = importPreNotification,
                    Created = new DateTime(2025, 4, 3, 10, 0, 0, DateTimeKind.Utc),
                    Updated = new DateTime(2025, 4, 3, 10, 15, 0, DateTimeKind.Utc),
                    ETag = "etag",
                }
            );

        var response = await client.GetStringAsync(
            TradeImportsDataApi.Testing.Endpoints.ImportPreNotifications.Get(ChedId)
        );

        await VerifyJson(response, _settings).UseStrictJson();
    }
}
