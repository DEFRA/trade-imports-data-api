using System.Net;
using System.Net.Http.Json;
using Defra.TradeImportsDataApi.Domain.Ipaffs;
using FluentAssertions;
using Xunit.Abstractions;

namespace Defra.TradeImportsDataApi.Api.IntegrationTests.Endpoints.ImportPreNotifications;

public class PutTests(ApiWebApplicationFactory factory, ITestOutputHelper outputHelper)
    : EndpointTestBase(factory, outputHelper)
{
    private const string ChedId = "chedId";

    [Fact]
    public async Task Put_WhenUnauthorized_ShouldBeUnauthorized()
    {
        var client = CreateClient(addDefaultAuthorizationHeader: false);

        var response = await client.PutAsJsonAsync(
            Testing.Endpoints.ImportPreNotifications.Put(ChedId),
            new ImportPreNotification()
        );

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Put_WhenReadOnly_ShouldBeForbidden()
    {
        var client = CreateClient(testUser: TestUser.ReadOnly);

        var response = await client.PutAsJsonAsync(
            Testing.Endpoints.ImportPreNotifications.Put(ChedId),
            new ImportPreNotification()
        );

        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
}
