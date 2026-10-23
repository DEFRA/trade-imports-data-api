using Argon;
using Defra.TradeImportsDataApi.Domain.Iuu;
using Defra.TradeImportsDataApi.Testing;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Defra.TradeImportsDataApi.Api.Client.Tests.Endpoints.IuuNotifications;

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
    public async Task GetIuuNotification_WhenNotFound_ShouldBeNull()
    {
        var result = await Subject.GetIuuNotification("unknown", CancellationToken.None);

        result.Should().BeNull();
    }

    [Fact]
    public async Task GetIuuNotification_WhenFound_ShouldNotBeNull()
    {
        const string iuuReference = "IUU.FR.2026.0000001";
        var created = new DateTime(2026, 10, 16, 9, 0, 0, DateTimeKind.Utc);
        var updated = created.AddMinutes(15);

        WireMock
            .Given(Request.Create().WithPath($"/iuu-notifications/{iuuReference}").UsingGet())
            .RespondWith(
                Response
                    .Create()
                    .WithBody(
                        JsonSerializer.Serialize(
                            new Defra.TradeImportsDataApi.Api.Endpoints.IuuNotifications.IuuNotificationResponse(
                                new IuuNotification
                                {
                                    Reference = iuuReference,
                                    LastUpdated = updated,
                                    IuuOption = "IUUOK",
                                },
                                created,
                                updated
                            )
                        )
                    )
                    .WithStatusCode(StatusCodes.Status200OK)
                    .WithHeader("ETag", "\"etag\"")
            );

        var result = await Subject.GetIuuNotification(iuuReference, CancellationToken.None);

        result.Should().NotBeNull();
        await Verify(result, _settings);
    }
}
