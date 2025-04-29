using Defra.TradeImportsDataApi.Api.Endpoints.Search;
using Defra.TradeImportsDataApi.Api.Services;
using FluentAssertions;

namespace Defra.TradeImportsDataApi.Api.Tests.Services;

public class SearchServiceTests
{
    [Fact]
    public async Task InitialSearchTest()
    {
        var subject = new SearchService();

        var response = await subject.Search(new SearchRequest(), CancellationToken.None);

        response.ImportPreNotifications.Length.Should().Be(3);
        response.CustomsDeclaration.Length.Should().Be(3);
    }
}
