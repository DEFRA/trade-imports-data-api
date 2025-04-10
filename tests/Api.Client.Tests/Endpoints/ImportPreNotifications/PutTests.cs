using Defra.TradeImportsDataApi.Testing;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using WireMock.Matchers;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Defra.TradeImportsDataApi.Api.Client.Tests.Endpoints.ImportPreNotifications;

public class PutTests(WireMockContext context) : WireMockTestBase<WireMockContext>(context)
{
    private TradeImportsDataApiClient Subject { get; } = new(context.HttpClient);

    [Fact]
    public async Task PutImportPreNotification_WhenNoEtag_ShouldNotBeNull()
    {
        const string chedId = "CHED";
        var data = new Domain.Ipaffs.ImportPreNotification { ReferenceNumber = chedId };
        WireMock
            .Given(
                Request
                    .Create()
                    .WithPath($"/import-pre-notifications/{chedId}")
                    .WithBody(JsonSerializer.Serialize(data))
                    .WithHeader("If-Match", "", MatchBehaviour.RejectOnMatch)
                    .UsingPut()
            )
            .RespondWith(Response.Create().WithStatusCode(StatusCodes.Status201Created));

        var act = async () => await Subject.PutImportPreNotification(chedId, data, etag: null, CancellationToken.None);

        await act.Should().NotThrowAsync();
    }

    [Fact]
    public async Task PutImportPreNotification_WhenHasEtag_ShouldNotBeNull()
    {
        const string chedId = "CHED";
        var data = new Domain.Ipaffs.ImportPreNotification { ReferenceNumber = chedId };
        WireMock
            .Given(
                Request
                    .Create()
                    .WithPath($"/import-pre-notifications/{chedId}")
                    .WithBody(JsonSerializer.Serialize(data))
                    .WithHeader("If-Match", "\"etag\"")
                    .UsingPut()
            )
            .RespondWith(Response.Create().WithStatusCode(StatusCodes.Status204NoContent));

        var act = async () =>
            await Subject.PutImportPreNotification(chedId, data, etag: "\"etag\"", CancellationToken.None);

        await act.Should().NotThrowAsync();
    }

    [Fact]
    public async Task PutImportPreNotification_WhenBadRequest_ShouldThrow()
    {
        const string chedId = "CHED";
        var data = new Domain.Ipaffs.ImportPreNotification { ReferenceNumber = chedId };
        WireMock
            .Given(
                Request
                    .Create()
                    .WithPath($"/import-pre-notifications/{chedId}")
                    .WithBody(JsonSerializer.Serialize(data))
                    .UsingPut()
            )
            .RespondWith(Response.Create().WithStatusCode(StatusCodes.Status400BadRequest));

        var act = async () => await Subject.PutImportPreNotification(chedId, data, etag: null, CancellationToken.None);

        await act.Should().ThrowAsync<HttpRequestException>();
    }
}
