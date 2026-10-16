using Defra.TradeImportsDataApi.Domain.Iuu;
using Defra.TradeImportsDataApi.Testing;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using WireMock.Matchers;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Defra.TradeImportsDataApi.Api.Client.Tests.Endpoints.IuuNotifications;

public class PutTests(WireMockContext context) : WireMockTestBase<WireMockContext>(context)
{
    private TradeImportsDataApiClient Subject { get; } = new(context.HttpClient);

    [Fact]
    public async Task PutIuuNotification_WhenNoEtag_ShouldNotBeNull()
    {
        const string iuuReference = "IUU.FR.2026.0000001";
        var data = new IuuNotification
        {
            Reference = iuuReference,
            LastUpdated = DateTime.Today,
            IuuOption = "IUUOK",
        };
        WireMock
            .Given(
                Request
                    .Create()
                    .WithPath($"/iuu-notifications/{iuuReference}")
                    .WithBody(JsonSerializer.Serialize(data))
                    .WithHeader("If-Match", "", MatchBehaviour.RejectOnMatch)
                    .UsingPut()
            )
            .RespondWith(Response.Create().WithStatusCode(StatusCodes.Status201Created));

        var act = async () => await Subject.PutIuuNotification(iuuReference, data, etag: null, CancellationToken.None);

        await act.Should().NotThrowAsync();
    }

    [Fact]
    public async Task PutIuuNotification_WhenHasEtag_ShouldNotBeNull()
    {
        const string iuuReference = "IUU.FR.2026.0000001";
        var data = new IuuNotification
        {
            Reference = iuuReference,
            LastUpdated = DateTime.Today,
            IuuOption = "IUUOK",
        };
        WireMock
            .Given(
                Request
                    .Create()
                    .WithPath($"/iuu-notifications/{iuuReference}")
                    .WithBody(JsonSerializer.Serialize(data))
                    .WithHeader("If-Match", "\"etag\"")
                    .UsingPut()
            )
            .RespondWith(Response.Create().WithStatusCode(StatusCodes.Status204NoContent));

        var act = async () =>
            await Subject.PutIuuNotification(iuuReference, data, etag: "\"etag\"", CancellationToken.None);

        await act.Should().NotThrowAsync();
    }

    [Fact]
    public async Task PutIuuNotification_WhenBadRequest_ShouldThrow()
    {
        const string iuuReference = "IUU.FR.2026.0000001";
        var data = new IuuNotification
        {
            Reference = iuuReference,
            LastUpdated = DateTime.Today,
            IuuOption = "IUUOK",
        };
        WireMock
            .Given(
                Request
                    .Create()
                    .WithPath($"/iuu-notifications/{iuuReference}")
                    .WithBody(JsonSerializer.Serialize(data))
                    .UsingPut()
            )
            .RespondWith(Response.Create().WithStatusCode(StatusCodes.Status400BadRequest));

        var act = async () => await Subject.PutIuuNotification(iuuReference, data, etag: null, CancellationToken.None);

        await act.Should().ThrowAsync<HttpRequestException>();
    }
}
