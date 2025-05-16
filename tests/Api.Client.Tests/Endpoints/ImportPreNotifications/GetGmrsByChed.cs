using Argon;
using Defra.TradeImportsDataApi.Data.Entities;
using Defra.TradeImportsDataApi.Domain.Gvms;
using Defra.TradeImportsDataApi.Testing;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Defra.TradeImportsDataApi.Api.Client.Tests.Endpoints.ImportPreNotifications;

public class GetGmrsByChed : WireMockTestBase<WireMockContext>
{
    private TradeImportsDataApiClient Subject { get; }

    private readonly VerifySettings _settings;

    public GetGmrsByChed(WireMockContext context)
        : base(context)
    {
        Subject = new TradeImportsDataApiClient(context.HttpClient);

        _settings = new VerifySettings();
        _settings.DontScrubGuids();
        _settings.DontScrubDateTimes();
        _settings.AddExtraSettings(settings => settings.DefaultValueHandling = DefaultValueHandling.Include);
    }

    [Fact]
    public async Task GetGmrsByChed_WhenNotFound_ShouldBeNull()
    {
        var result = await Subject.GetGmrsByChedId("unknown", CancellationToken.None);

        result.Should().BeNull();
    }

    [Fact]
    public async Task GetGmrsByChed_WhenFound_ShouldNotBeNull()
    {
        const string chedId = "CHED";
        var created = new DateTime(2025, 4, 7, 11, 0, 0, DateTimeKind.Utc);
        var updated = created.AddMinutes(15);

        WireMock
            .Given(Request.Create().WithPath($"/import-pre-notifications/{chedId}/gmrs").UsingGet())
            .RespondWith(
                Response
                    .Create()
                    .WithBody(
                        JsonSerializer.Serialize(
                            new List<GmrEntity>
                            {
                                new()
                                {
                                    Id = "123",
                                    Gmr = new Gmr(),
                                    Created = created,
                                    Updated = updated,
                                    ETag = "etag",
                                },
                            }
                        )
                    )
                    .WithStatusCode(StatusCodes.Status200OK)
            );

        var result = await Subject.GetGmrsByChedId(chedId, CancellationToken.None);

        result.Should().NotBeNull();
        await Verify(result, _settings);
    }
}
