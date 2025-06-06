using Argon;
using Defra.TradeImportsDataApi.Domain.CustomsDeclaration;
using Defra.TradeImportsDataApi.Testing;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Defra.TradeImportsDataApi.Api.Client.Tests.Endpoints.ImportPreNotifications;

public class GetCustomsDeclarationsByChedTests : WireMockTestBase<WireMockContext>
{
    private TradeImportsDataApiClient Subject { get; }

    private readonly VerifySettings _settings;

    public GetCustomsDeclarationsByChedTests(WireMockContext context)
        : base(context)
    {
        Subject = new TradeImportsDataApiClient(context.HttpClient);

        _settings = new VerifySettings();
        _settings.DontScrubGuids();
        _settings.DontScrubDateTimes();
        _settings.AddExtraSettings(settings => settings.DefaultValueHandling = DefaultValueHandling.Include);
    }

    [Fact]
    public async Task GetCustomDeclarationsByChed_WhenFound_ShouldNotBeNull()
    {
        const string chedId = "CHED";
        var created = new DateTime(2025, 4, 7, 11, 0, 0, DateTimeKind.Utc);
        var updated = created.AddMinutes(15);

        WireMock
            .Given(Request.Create().WithPath($"/import-pre-notifications/{chedId}/customs-declarations").UsingGet())
            .RespondWith(
                Response
                    .Create()
                    .WithBody(
                        JsonSerializer.Serialize(
                            new Defra.TradeImportsDataApi.Api.Endpoints.CustomsDeclarations.CustomsDeclarationsResponse(
                                new List<Defra.TradeImportsDataApi.Api.Endpoints.CustomsDeclarations.CustomsDeclarationResponse>
                                {
                                    new Api.Endpoints.CustomsDeclarations.CustomsDeclarationResponse(
                                        "mrn",
                                        new ClearanceRequest(),
                                        null,
                                        null,
                                        null,
                                        created,
                                        updated
                                    ),
                                }
                            )
                        )
                    )
                    .WithStatusCode(StatusCodes.Status200OK)
            );

        var result = await Subject.GetCustomsDeclarationsByChedId(chedId, CancellationToken.None);

        result.Should().NotBeNull();
        await Verify(result, _settings);
    }
}
