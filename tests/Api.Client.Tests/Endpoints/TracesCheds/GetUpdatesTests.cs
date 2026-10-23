using Argon;
using Defra.TradeImportsDataApi.Testing;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Defra.TradeImportsDataApi.Api.Client.Tests.Endpoints.TracesCheds;

public class GetUpdatesTests : WireMockTestBase<WireMockContext>
{
    private TradeImportsDataApiClient Subject { get; }

    private readonly VerifySettings _settings;

    public GetUpdatesTests(WireMockContext context)
        : base(context)
    {
        Subject = new TradeImportsDataApiClient(context.HttpClient);

        _settings = new VerifySettings();
        _settings.DontScrubGuids();
        _settings.DontScrubDateTimes();
        _settings.AddExtraSettings(settings => settings.DefaultValueHandling = DefaultValueHandling.Include);
    }

    [Fact]
    public async Task GetTracesChedUpdates_WhenFound_ShouldReturnUpdates()
    {
        var from = new DateTime(2025, 5, 21, 8, 0, 0, DateTimeKind.Utc);
        var to = from.AddHours(1);

        WireMock
            .Given(
                Request
                    .Create()
                    .WithPath("/traces-ched-updates")
                    .WithParam("from", $"{from:O}")
                    .WithParam("to", $"{to:O}")
                    .WithParam("page", "1")
                    .WithParam("pageSize", "10")
                    .UsingGet()
            )
            .RespondWith(
                Response
                    .Create()
                    .WithBody(
                        JsonSerializer.Serialize(
                            new Defra.TradeImportsDataApi.Api.Endpoints.TracesChed.TracesChedUpdatesResponse(
                                [
                                    new Defra.TradeImportsDataApi.Api.Endpoints.TracesChed.TracesChedUpdateResponse(
                                        "CHEDA.GB.2026.1234567",
                                        from.AddMinutes(15)
                                    ),
                                ],
                                Total: 1,
                                Page: 1,
                                PageSize: 10
                            )
                        )
                    )
                    .WithStatusCode(StatusCodes.Status200OK)
            );

        var result = await Subject.GetTracesChedUpdates(
            new TracesChedUpdatesRequest
            {
                From = from,
                To = to,
                Page = 1,
                PageSize = 10,
            },
            CancellationToken.None
        );

        result.Should().NotBeNull();
        await Verify(result, _settings);
    }
}
