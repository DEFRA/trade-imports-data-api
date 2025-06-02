using System.Text.Json;
using Defra.TradeImportsDataApi.Domain.Errors;
using Defra.TradeImportsDataApi.Testing;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using WireMock.Matchers;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;

namespace Defra.TradeImportsDataApi.Api.Client.Tests.Endpoints.ProcessingErrors;

public class PutTests(WireMockContext context) : WireMockTestBase<WireMockContext>(context)
{
    private TradeImportsDataApiClient Subject { get; } = new(context.HttpClient);

    [Fact]
    public async Task PutProcessingError_WhenNoEtag_ShouldNotBeNull()
    {
        const string mrn = "mrn";
        var data = Array.Empty<ProcessingError>();
        WireMock
            .Given(
                Request
                    .Create()
                    .WithPath($"/processing-errors/{mrn}")
                    .WithBody(JsonSerializer.Serialize(data))
                    .WithHeader("If-Match", "", MatchBehaviour.RejectOnMatch)
                    .UsingPut()
            )
            .RespondWith(Response.Create().WithStatusCode(StatusCodes.Status201Created));

        var act = async () => await Subject.PutProcessingError(mrn, [], etag: null, CancellationToken.None);

        await act.Should().NotThrowAsync();
    }

    [Fact]
    public async Task PutProcessingError_WhenHasEtag_ShouldNotBeNull()
    {
        const string mrn = "mrn";
        var data = Array.Empty<ProcessingError>();
        WireMock
            .Given(
                Request
                    .Create()
                    .WithPath($"/processing-errors/{mrn}")
                    .WithBody(JsonSerializer.Serialize(data))
                    .WithHeader("If-Match", "\"etag\"")
                    .UsingPut()
            )
            .RespondWith(Response.Create().WithStatusCode(StatusCodes.Status204NoContent));

        var act = async () => await Subject.PutProcessingError(mrn, data, etag: "\"etag\"", CancellationToken.None);

        await act.Should().NotThrowAsync();
    }

    [Fact]
    public async Task PutProcessingError_WhenBadRequest_ShouldThrow()
    {
        const string mrn = "mrn";
        var data = Array.Empty<ProcessingError>();
        WireMock
            .Given(
                Request
                    .Create()
                    .WithPath($"/processing-errors/{mrn}")
                    .WithBody(JsonSerializer.Serialize(data))
                    .UsingPut()
            )
            .RespondWith(Response.Create().WithStatusCode(StatusCodes.Status400BadRequest));

        var act = async () => await Subject.PutProcessingError(mrn, data, etag: null, CancellationToken.None);

        await act.Should().ThrowAsync<HttpRequestException>();
    }
}
