using System.Globalization;
using System.Net;
using System.Text.Json;
using Defra.TradeImportsDataApi.Api.Data;
using Defra.TradeImportsDataApi.Api.Tests.Utils.InMemoryData;
using Defra.TradeImportsDataApi.Data;
using Defra.TradeImportsDataApi.Data.Entities;
using Defra.TradeImportsDataApi.Domain.CustomsDeclaration;
using Defra.TradeImportsDataApi.Testing;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using WireMock.Server;
using Xunit.Abstractions;

namespace Defra.TradeImportsDataApi.Api.Tests.Endpoints.Reporting;

public class GetTests : EndpointTestBase, IClassFixture<WireMockContext>
{
    private WireMockServer WireMock { get; }
    private readonly VerifySettings _settings;
    private static readonly JsonSerializerOptions s_jsonOptions = new() { WriteIndented = true };

    private MemoryDbContext MemoryDbContext { get; } = new MemoryDbContext();

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

        services.AddTransient<IDbContext>(_ => MemoryDbContext);
    }

    [Fact]
    public async Task Get_WhenUnauthorized_ShouldBeUnauthorized()
    {
        var client = CreateClient(addDefaultAuthorizationHeader: false);

        var response = await client.GetAsync(
            TradeImportsDataApi.Testing.Endpoints.Reporting.ManualRelease(DateTime.UtcNow, DateTime.UtcNow)
        );

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Get_WhenWriteOnly_ShouldBeForbidden()
    {
        var client = CreateClient(testUser: TestUser.WriteOnly);

        var response = await client.GetAsync(
            TradeImportsDataApi.Testing.Endpoints.Reporting.ManualRelease(DateTime.UtcNow, DateTime.UtcNow)
        );

        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task Get_WhenFromIsGreaterThanTo_ShouldBeBadRequest()
    {
        var client = CreateClient();

        var response = await client.GetAsync(
            TradeImportsDataApi.Testing.Endpoints.Reporting.ManualRelease(DateTime.UtcNow.AddHours(1), DateTime.UtcNow)
        );

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Get_WheSpanIsMoreThan31Days_ShouldBeBadRequest()
    {
        var client = CreateClient();

        var response = await client.GetAsync(
            TradeImportsDataApi.Testing.Endpoints.Reporting.ManualRelease(DateTime.UtcNow.AddDays(32), DateTime.UtcNow)
        );

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Get_WhenAuthorized_Report_ShouldBeOk()
    {
        var body = EmbeddedResource.GetBody(GetType(), "GetTests_DomainExample.json");
        var customsDeclaration =
            JsonSerializer.Deserialize<CustomsDeclaration>(body, s_jsonOptions)
            ?? throw new InvalidOperationException();

        var client = CreateClient();
        MemoryDbContext.CustomsDeclarations.AddTestData(
            new CustomsDeclarationEntity
            {
                Id = "Mrn1",
                ClearanceRequest = customsDeclaration.ClearanceRequest,
                ClearanceDecision = customsDeclaration.ClearanceDecision,
                Finalisation = customsDeclaration.Finalisation,
                ExternalErrors = customsDeclaration.ExternalErrors,
                Created = new DateTime(2025, 4, 3, 10, 0, 0, DateTimeKind.Utc),
                Updated = new DateTime(2025, 4, 3, 10, 15, 0, DateTimeKind.Utc),
                ETag = "etag",
            }
        );

        customsDeclaration =
            JsonSerializer.Deserialize<CustomsDeclaration>(body, s_jsonOptions)
            ?? throw new InvalidOperationException();
        customsDeclaration.Finalisation!.IsManualRelease = false;
        MemoryDbContext.CustomsDeclarations.AddTestData(
            new CustomsDeclarationEntity
            {
                Id = "Mrn2",
                ClearanceRequest = customsDeclaration.ClearanceRequest,
                ClearanceDecision = customsDeclaration.ClearanceDecision,
                Finalisation = customsDeclaration.Finalisation,
                ExternalErrors = customsDeclaration.ExternalErrors,
                Created = new DateTime(2025, 4, 3, 10, 0, 0, DateTimeKind.Utc),
                Updated = new DateTime(2025, 4, 3, 10, 15, 0, DateTimeKind.Utc),
                ETag = "etag",
            }
        );

        customsDeclaration =
            JsonSerializer.Deserialize<CustomsDeclaration>(body, s_jsonOptions)
            ?? throw new InvalidOperationException();
        customsDeclaration.Finalisation!.MessageSentAt = DateTime.UtcNow.AddYears(-6);
        MemoryDbContext.CustomsDeclarations.AddTestData(
            new CustomsDeclarationEntity
            {
                Id = "Mrn3",
                ClearanceRequest = customsDeclaration.ClearanceRequest,
                ClearanceDecision = customsDeclaration.ClearanceDecision,
                Finalisation = customsDeclaration.Finalisation,
                ExternalErrors = customsDeclaration.ExternalErrors,
                Created = new DateTime(2025, 4, 3, 10, 0, 0, DateTimeKind.Utc),
                Updated = new DateTime(2025, 4, 3, 10, 15, 0, DateTimeKind.Utc),
                ETag = "etag",
            }
        );

        var response = await client.GetAsync(
            TradeImportsDataApi.Testing.Endpoints.Reporting.ManualRelease(
                DateTime.Parse("2024-02-16", CultureInfo.CurrentCulture),
                DateTime.Parse("2024-02-20", CultureInfo.CurrentCulture)
            )
        );

        await VerifyJson(await response.Content.ReadAsStringAsync(), _settings);
    }
}
