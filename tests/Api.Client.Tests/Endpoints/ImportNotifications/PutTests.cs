using Argon;
using Defra.TradeImportsDataApi.Testing;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging.Abstractions;
using WireMock.Matchers;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Defra.TradeImportsDataApi.Api.Client.Tests.Endpoints.ImportNotifications;

public class PutTests : WireMockTestBase<WireMockContext>
{
    private TradeImportsDataApiClient Subject { get; }

    private readonly VerifySettings _settings;

    public PutTests(WireMockContext context)
        : base(context)
    {
        Subject = new TradeImportsDataApiClient(context.HttpClient, NullLogger<TradeImportsDataApiClient>.Instance);

        _settings = new VerifySettings();
        _settings.DontScrubGuids();
        _settings.DontScrubDateTimes();
        _settings.AddExtraSettings(settings => settings.DefaultValueHandling = DefaultValueHandling.Include);
    }

    [Fact]
    public async Task PutImportNotification_NoEtag_ShouldNotBeNull()
    {
        const string chedId = "CHED";
        var created = new DateTime(2025, 4, 7, 11, 0, 0, DateTimeKind.Utc);
        var updated = created.AddMinutes(15);
        var data = new Domain.Ipaffs.ImportNotification { ReferenceNumber = chedId };
        WireMock
            .Given(
                Request
                    .Create()
                    .WithPath($"/import-notifications/{chedId}")
                    .WithBody(JsonSerializer.Serialize(data))
                    .WithHeader("If-Match", "", MatchBehaviour.RejectOnMatch)
                    .UsingPut()
            )
            .RespondWith(
                Response
                    .Create()
                    .WithBody(
                        JsonSerializer.Serialize(
                            new Defra.TradeImportsDataApi.Api.Endpoints.ImportNotifications.ImportNotificationResponse(
                                data,
                                created,
                                updated
                            )
                        )
                    )
                    .WithStatusCode(StatusCodes.Status200OK)
                    .WithHeader("ETag", "\"etag\"")
            );

        var result = await Subject.PutImportNotification(chedId, data, etag: null, CancellationToken.None);

        result.Should().NotBeNull();
        await Verify(result, _settings);
    }

    [Fact]
    public async Task PutImportNotification_WithEtag_ShouldNotBeNull()
    {
        const string chedId = "CHED";
        var created = new DateTime(2025, 4, 7, 11, 0, 0, DateTimeKind.Utc);
        var updated = created.AddMinutes(15);
        var data = new Domain.Ipaffs.ImportNotification { ReferenceNumber = chedId };
        WireMock
            .Given(
                Request
                    .Create()
                    .WithPath($"/import-notifications/{chedId}")
                    .WithBody(JsonSerializer.Serialize(data))
                    .WithHeader("If-Match", "\"etag\"")
                    .UsingPut()
            )
            .RespondWith(
                Response
                    .Create()
                    .WithBody(
                        JsonSerializer.Serialize(
                            new Defra.TradeImportsDataApi.Api.Endpoints.ImportNotifications.ImportNotificationResponse(
                                data,
                                created,
                                updated
                            )
                        )
                    )
                    .WithStatusCode(StatusCodes.Status200OK)
                    .WithHeader("ETag", "\"etag\"")
            );

        var result = await Subject.PutImportNotification(chedId, data, etag: "\"etag\"", CancellationToken.None);

        result.Should().NotBeNull();
        await Verify(result, _settings);
    }
}
