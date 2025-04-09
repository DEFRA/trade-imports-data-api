using Defra.TradeImportsDataApi.Domain.CustomsDeclaration;
using Defra.TradeImportsDataApi.Testing;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using WireMock.Matchers;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Defra.TradeImportsDataApi.Api.Client.Tests.Endpoints.CustomsDeclarations;

public class PutTests(WireMockContext context) : WireMockTestBase<WireMockContext>(context)
{
    private TradeImportsDataApiClient Subject { get; } = new(context.HttpClient);

    [Fact]
    public async Task PutCustomsDeclaration_WhenNoEtag_ShouldNotBeNull()
    {
        const string mrn = "mrn";
        var data = new CustomsDeclaration { ClearanceRequest = new ClearanceRequest() };
        WireMock
            .Given(
                Request
                    .Create()
                    .WithPath($"/customs-declarations/{mrn}")
                    .WithBody(JsonSerializer.Serialize(data))
                    .WithHeader("If-Match", "", MatchBehaviour.RejectOnMatch)
                    .UsingPut()
            )
            .RespondWith(Response.Create().WithStatusCode(StatusCodes.Status201Created));

        var act = async () => await Subject.PutCustomsDeclaration(mrn, data, etag: null, CancellationToken.None);

        await act.Should().NotThrowAsync();
    }

    [Fact]
    public async Task PutCustomsDeclaration_WhenHasEtag_ShouldNotBeNull()
    {
        const string mrn = "mrn";
        var data = new CustomsDeclaration { ClearanceRequest = new ClearanceRequest() };
        WireMock
            .Given(
                Request
                    .Create()
                    .WithPath($"/customs-declarations/{mrn}")
                    .WithBody(JsonSerializer.Serialize(data))
                    .WithHeader("If-Match", "\"etag\"")
                    .UsingPut()
            )
            .RespondWith(Response.Create().WithStatusCode(StatusCodes.Status204NoContent));

        var act = async () => await Subject.PutCustomsDeclaration(mrn, data, etag: "\"etag\"", CancellationToken.None);

        await act.Should().NotThrowAsync();
    }

    [Fact]
    public async Task PutCustomsDeclaration_WhenBadRequest_ShouldThrow()
    {
        const string mrn = "mrn";
        var data = new CustomsDeclaration { ClearanceRequest = new ClearanceRequest() };
        WireMock
            .Given(
                Request
                    .Create()
                    .WithPath($"/customs-declarations/{mrn}")
                    .WithBody(JsonSerializer.Serialize(data))
                    .UsingPut()
            )
            .RespondWith(Response.Create().WithStatusCode(StatusCodes.Status400BadRequest));

        var act = async () => await Subject.PutCustomsDeclaration(mrn, data, etag: null, CancellationToken.None);

        await act.Should().ThrowAsync<HttpRequestException>();
    }
}
