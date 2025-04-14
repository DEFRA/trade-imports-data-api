using Argon;
using Defra.TradeImportsDataApi.Testing;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Defra.TradeImportsDataApi.Api.Client.Tests.Endpoints.CustomsDeclarations;

public class GetImportPreNotificationsTests : WireMockTestBase<WireMockContext>
{
    private TradeImportsDataApiClient Subject { get; }

    private readonly VerifySettings _settings;

    public GetImportPreNotificationsTests(WireMockContext context)
        : base(context)
    {
        Subject = new TradeImportsDataApiClient(context.HttpClient);

        _settings = new VerifySettings();
        _settings.DontScrubGuids();
        _settings.DontScrubDateTimes();
        _settings.AddExtraSettings(settings => settings.DefaultValueHandling = DefaultValueHandling.Include);
    }

    [Fact]
    public async Task GetCustomsDeclaration_WhenNotFound_ShouldBeNull()
    {
        var result = await Subject.GetImportPreNotificationsByMrn("unknown", CancellationToken.None);

        result.Should().BeNull();
    }

    [Fact]
    public async Task GetCustomsDeclaration_WhenFound_ShouldNotBeNull()
    {
        const string mrn = "mrn";
        var created = new DateTime(2025, 4, 7, 11, 0, 0, DateTimeKind.Utc);
        var updated = created.AddMinutes(15);
        WireMock
            .Given(Request.Create().WithPath($"/customs-declarations/{mrn}/import-pre-notifications").UsingGet())
            .RespondWith(
                Response
                    .Create()
                    .WithBody(
                        JsonSerializer.Serialize(
                            new List<Api.Endpoints.ImportPreNotifications.ImportPreNotificationResponse>()
                            {
                                new Api.Endpoints.ImportPreNotifications.ImportPreNotificationResponse(
                                    new Domain.Ipaffs.ImportPreNotification { ReferenceNumber = "ched" },
                                    created,
                                    updated
                                ),
                            }
                        )
                    )
                    .WithStatusCode(StatusCodes.Status200OK)
            );

        var result = await Subject.GetImportPreNotificationsByMrn(mrn, CancellationToken.None);

        result.Should().NotBeNull();
        await Verify(result, _settings);
    }
}
