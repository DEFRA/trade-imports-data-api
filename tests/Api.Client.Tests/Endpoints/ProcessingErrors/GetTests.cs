using Argon;
using Defra.TradeImportsDataApi.Domain.Errors;
using Defra.TradeImportsDataApi.Testing;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Defra.TradeImportsDataApi.Api.Client.Tests.Endpoints.ProcessingErrors;

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
    public async Task GetProcessingError_WhenNotFound_ShouldBeNull()
    {
        var result = await Subject.GetProcessingError("unknown", CancellationToken.None);

        result.Should().BeNull();
    }

    [Fact]
    public async Task GetProcessingError_WhenFound_ShouldNotBeNull()
    {
        const string mrn = "mrn";
        var created = new DateTime(2025, 4, 7, 11, 0, 0, DateTimeKind.Utc);
        var updated = created.AddMinutes(15);
        WireMock
            .Given(Request.Create().WithPath($"/processing-errors/{mrn}").UsingGet())
            .RespondWith(
                Response
                    .Create()
                    .WithBody(
                        JsonSerializer.Serialize(
                            new Defra.TradeImportsDataApi.Api.Endpoints.ProcessingErrors.ProcessingErrorResponse(
                                mrn,
                                [
                                    new ProcessingError
                                    {
                                        ExternalCorrelationId = "80000000",
                                        ExternalSourceCorrelationId = "1800000",
                                        ExternalVersion = 1,
                                        Created = created,
                                        Errors = [
                                            new ErrorItem
                                            {
                                                Code = "error-code",
                                                Message = "error-message"
                                            }
                                        ],
                                        Message = "error-message"
                                    }
                                ],
                                created,
                                updated
                            )
                        )
                    )
                    .WithStatusCode(StatusCodes.Status200OK)
                    .WithHeader("ETag", "\"etag\"")
            );

        var result = await Subject.GetProcessingError(mrn, CancellationToken.None);

        result.Should().NotBeNull();
        await Verify(result, _settings);
    }
}
