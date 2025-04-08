using Argon;
using Defra.TradeImportsDataApi.Testing;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging.Abstractions;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Defra.TradeImportsDataApi.Api.Client.Tests.Endpoints.ImportNotifications;

public class GetTests : WireMockTestBase<WireMockContext>
{
    private TradeImportsDataApiClient Subject { get; }

    private readonly VerifySettings _settings;

    public GetTests(WireMockContext context)
        : base(context)
    {
        Subject = new TradeImportsDataApiClient(context.HttpClient, NullLogger<TradeImportsDataApiClient>.Instance);

        _settings = new VerifySettings();
        _settings.DontScrubGuids();
        _settings.DontScrubDateTimes();
        _settings.AddExtraSettings(settings => settings.DefaultValueHandling = DefaultValueHandling.Include);
    }

    [Fact]
    public async Task GetImportNotification_NotFound_ShouldBeNull()
    {
        var result = await Subject.GetImportNotification("unknown", CancellationToken.None);

        result.Should().BeNull();
    }

    [Fact]
    public async Task GetImportNotification_Found_ShouldNotBeNull()
    {
        const string chedId = "CHED";
        var created = new DateTime(2025, 4, 7, 11, 0, 0, DateTimeKind.Utc);
        var updated = created.AddMinutes(15);
        WireMock
            .Given(Request.Create().WithPath($"/import-notifications/{chedId}").UsingGet())
            .RespondWith(
                Response
                    .Create()
                    .WithBody(
                        JsonSerializer.Serialize(
                            new Defra.TradeImportsDataApi.Api.Endpoints.ImportNotifications.ImportNotificationResponse(
                                new Domain.Ipaffs.ImportNotification { ReferenceNumber = chedId },
                                created,
                                updated
                            )
                        )
                    )
                    .WithStatusCode(StatusCodes.Status200OK)
                    .WithHeader("ETag", "\"etag\"")
            );

        var result = await Subject.GetImportNotification(chedId, CancellationToken.None);

        result.Should().NotBeNull();
        await Verify(result, _settings);
    }
}
