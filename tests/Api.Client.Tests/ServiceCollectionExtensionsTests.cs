using System.Net;
using System.Net.Http.Headers;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Defra.TradeImportsDataApi.Api.Client.Tests;

public class ServiceCollectionExtensionsTests
{
    [Fact]
    public void AddTradeImportsDataApiClient_DefaultBehaviour()
    {
        var services = new ServiceCollection();

        services.AddTradeImportsDataApiClient();

        using var serviceProvider = services.BuildServiceProvider();

        var httpClient = serviceProvider
            .GetRequiredService<IHttpClientFactory>()
            .CreateClient(nameof(ITradeImportsDataApiClient));

        httpClient.Should().NotBeNull();
    }

    [Theory]
    [InlineData("http://localhost", 1, 1)]
    [InlineData("https://localhost", 2, 0)]
    public void AddTradeImportsDataApiClient_ConsumptionBehaviour(
        string baseAddress,
        int expectedMajor,
        int expectedMinor
    )
    {
        var services = new ServiceCollection();
        var authHeader = Convert.ToBase64String("username:password"u8.ToArray());

        services.Configure<TestOptions>(options =>
        {
            options.BaseUrl = baseAddress;
            options.BasicAuthCredential = authHeader;
        });
        services
            .AddTradeImportsDataApiClient()
            .ConfigureHttpClient(
                (sp, client) =>
                {
                    // This configuration would be in the consuming project of the client.
                    // It shows an example of what might be seen.

                    var options = sp.GetRequiredService<IOptions<TestOptions>>().Value;

                    client.BaseAddress = new Uri(options.BaseUrl);

                    if (options.BasicAuthCredential is not null)
                    {
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                            "Basic",
                            options.BasicAuthCredential
                        );
                    }

                    if (client.BaseAddress.Scheme.Equals("https", StringComparison.OrdinalIgnoreCase))
                        client.DefaultRequestVersion = HttpVersion.Version20;
                }
            );

        using var serviceProvider = services.BuildServiceProvider();

        var httpClient = serviceProvider
            .GetRequiredService<IHttpClientFactory>()
            .CreateClient(nameof(ITradeImportsDataApiClient));

        httpClient.Should().NotBeNull();
        httpClient.Should().NotBeNull();
        httpClient.BaseAddress.Should().Be(new Uri(baseAddress));
        httpClient.DefaultRequestVersion.Should().Be(new Version(expectedMajor, expectedMinor));
        httpClient.DefaultRequestHeaders.Authorization?.Scheme.Should().Be("Basic");
        httpClient.DefaultRequestHeaders.Authorization?.Parameter.Should().Be(authHeader);
    }

    public class TestOptions
    {
        public required string BaseUrl { get; set; }
        public string? BasicAuthCredential { get; set; }
    }
}
