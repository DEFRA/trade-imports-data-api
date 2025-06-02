using System.Net;
using System.Text.Json;
using AutoFixture;
using Defra.TradeImportsDataApi.Api.Services;
using Defra.TradeImportsDataApi.Data.Entities;
using Defra.TradeImportsDataApi.Domain.CustomsDeclaration;
using Defra.TradeImportsDataApi.Testing;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using WireMock.Server;
using Xunit.Abstractions;

namespace Defra.TradeImportsDataApi.Api.Tests.Endpoints.CustomsDeclarations;

public class GetTests : EndpointTestBase, IClassFixture<WireMockContext>
{
    private ICustomsDeclarationService MockCustomsDeclarationService { get; } =
        Substitute.For<ICustomsDeclarationService>();
    private WireMockServer WireMock { get; }
    private const string Mrn = "mrn";
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
        _settings.DontIgnoreEmptyCollections();
    }

    protected override void ConfigureTestServices(IServiceCollection services)
    {
        base.ConfigureTestServices(services);

        services.AddTransient<ICustomsDeclarationService>(_ => MockCustomsDeclarationService);
    }

    [Fact]
    public async Task Get_WhenNotFound_ShouldNotBeFound()
    {
        var client = CreateClient();

        var response = await client.GetAsync(TradeImportsDataApi.Testing.Endpoints.CustomsDeclarations.Get(Mrn));

        await VerifyJson(await response.Content.ReadAsStringAsync(), _settings);
    }

    [Fact]
    public async Task Get_WhenFound_ShouldReturnContent()
    {
        var client = CreateClient();
        MockCustomsDeclarationService
            .GetCustomsDeclaration(Mrn, Arg.Any<CancellationToken>())
            .Returns(
                new CustomsDeclarationEntity
                {
                    Id = Mrn,
                    ClearanceRequest = new ClearanceRequest(),
                    Created = new DateTime(2025, 4, 3, 10, 0, 0, DateTimeKind.Utc),
                    Updated = new DateTime(2025, 4, 3, 10, 15, 0, DateTimeKind.Utc),
                    ETag = "etag",
                }
            );

        var response = await client.GetAsync(TradeImportsDataApi.Testing.Endpoints.CustomsDeclarations.Get(Mrn));

        await VerifyJson(await response.Content.ReadAsStringAsync(), _settings)
            .UseMethodName(nameof(Get_WhenFound_ShouldReturnContent));
        await Verify(response, _settings).UseMethodName($"{nameof(Get_WhenFound_ShouldReturnContent)}_response");
    }

    [Fact]
    public async Task Get_WhenUnauthorized_ShouldBeUnauthorized()
    {
        var client = CreateClient(addDefaultAuthorizationHeader: false);

        var response = await client.GetAsync(TradeImportsDataApi.Testing.Endpoints.CustomsDeclarations.Get(Mrn));

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Get_WhenWriteOnly_ShouldBeForbidden()
    {
        var client = CreateClient(testUser: TestUser.WriteOnly);

        var response = await client.GetAsync(TradeImportsDataApi.Testing.Endpoints.CustomsDeclarations.Get(Mrn));

        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task Get_WhenGenerating_GetTests_DomainExample_ShouldSerialize()
    {
        var fixture = new Fixture();
        var customsDeclaration = fixture.Create<CustomsDeclaration>();
        var serialized = JsonSerializer.Serialize(customsDeclaration, s_jsonOptions);

        // Take this file and replace GetTests_DomainExample.json when needed or
        // take the parts that have changed/been added to minimise the amount of unnecessary changes
        await File.WriteAllTextAsync(
            $"{nameof(Get_WhenGenerating_GetTests_DomainExample_ShouldSerialize)}_CustomsDeclaration.json",
            serialized
        );

        serialized.Should().NotBeNull();
    }

    [Fact]
    public async Task Get_WhenReturningDomainExample_ShouldBeCorrectJson()
    {
        // See test above for generation of new content
        var body = EmbeddedResource.GetBody(GetType(), "GetTests_DomainExample.json");
        var customsDeclaration =
            JsonSerializer.Deserialize<CustomsDeclaration>(body, s_jsonOptions)
            ?? throw new InvalidOperationException();
        var client = CreateClient();
        MockCustomsDeclarationService
            .GetCustomsDeclaration(Mrn, Arg.Any<CancellationToken>())
            .Returns(
                new CustomsDeclarationEntity
                {
                    Id = Mrn,
                    ClearanceRequest = customsDeclaration.ClearanceRequest,
                    ClearanceDecision = customsDeclaration.ClearanceDecision,
                    Finalisation = customsDeclaration.Finalisation,
                    ExternalError = customsDeclaration.ExternalError,
                    Created = new DateTime(2025, 4, 3, 10, 0, 0, DateTimeKind.Utc),
                    Updated = new DateTime(2025, 4, 3, 10, 15, 0, DateTimeKind.Utc),
                    ETag = "etag",
                }
            );

        var response = await client.GetStringAsync(TradeImportsDataApi.Testing.Endpoints.CustomsDeclarations.Get(Mrn));

        await VerifyJson(response, _settings).UseStrictJson();
    }
}
