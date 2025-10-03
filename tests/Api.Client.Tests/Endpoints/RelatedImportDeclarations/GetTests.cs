using Argon;
using Defra.TradeImportsDataApi.Domain.CustomsDeclaration;
using Defra.TradeImportsDataApi.Domain.Gvms;
using Defra.TradeImportsDataApi.Domain.Ipaffs;
using Defra.TradeImportsDataApi.Testing;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Defra.TradeImportsDataApi.Api.Client.Tests.Endpoints.RelatedImportDeclarations;

public class GetTests : WireMockTestBase<WireMockContext>
{
    private TradeImportsDataApiClient Subject { get; }

    private readonly VerifySettings _settings;

    public GetTests(WireMockContext context)
        : base(context)
    {
        Subject = new TradeImportsDataApiClient(context.HttpClient);

        _settings = new VerifySettings();
        _settings.DontScrubGuids();
        _settings.DontScrubDateTimes();
        _settings.AddExtraSettings(settings => settings.DefaultValueHandling = DefaultValueHandling.Include);
    }

    [Fact]
    public async Task WhenNotFound_ShouldBeEmpty()
    {
        WireMock
            .Given(Request.Create().WithPath("/related-import-declarations").UsingGet())
            .RespondWith(
                Response
                    .Create()
                    .WithBody(JsonSerializer.Serialize(new RelatedImportDeclarationsResponse([], [], [])))
                    .WithStatusCode(StatusCodes.Status200OK)
            );

        var result = await Subject.RelatedImportDeclarations(
            new RelatedImportDeclarationsRequest(),
            CancellationToken.None
        );

        result.Should().NotBeNull();
        result.CustomsDeclarations.Length.Should().Be(0);
        result.ImportPreNotifications.Length.Should().Be(0);
    }

    [Fact]
    public async Task WhenFound_ShouldNotBeNull()
    {
        const string mrn = "mrn";
        var created = new DateTime(2025, 4, 7, 11, 0, 0, DateTimeKind.Utc);
        var updated = created.AddMinutes(15);

        WireMock
            .Given(Request.Create().WithPath("/related-import-declarations").UsingGet())
            .RespondWith(
                Response
                    .Create()
                    .WithBody(
                        JsonSerializer.Serialize(
                            new RelatedImportDeclarationsResponse(
                                [
                                    new CustomsDeclarationResponse(
                                        mrn,
                                        new ClearanceRequest(),
                                        null,
                                        null,
                                        null,
                                        created,
                                        updated
                                    ),
                                ],
                                [new ImportPreNotificationResponse(new ImportPreNotification(), created, updated)],
                                [new GmrResponse(new Gmr(), created, updated)]
                            )
                        )
                    )
                    .WithStatusCode(StatusCodes.Status200OK)
            );

        var result = await Subject.RelatedImportDeclarations(
            new RelatedImportDeclarationsRequest(),
            CancellationToken.None
        );

        result.Should().NotBeNull();
        await Verify(result, _settings);
    }
}
