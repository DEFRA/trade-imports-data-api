using System.Net.Http.Headers;
using System.Text;
using Defra.TradeImportsDataApi.Api.Authentication;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace Defra.TradeImportsDataApi.Api.IntegrationTests.Endpoints;

public class EndpointTestBase : IClassFixture<ApiWebApplicationFactory>
{
    private readonly ApiWebApplicationFactory _factory;

    protected EndpointTestBase(ApiWebApplicationFactory factory, ITestOutputHelper outputHelper)
    {
        _factory = factory;
        _factory.OutputHelper = outputHelper;
        _factory.ConfigureHostConfiguration = ConfigureHostConfiguration;
    }

    /// <summary>
    /// Use this to inject configuration before Host is created.
    /// </summary>
    /// <param name="config"></param>
    protected virtual void ConfigureHostConfiguration(IConfigurationBuilder config) { }

    /// <summary>
    /// Use this to override DI services.
    /// </summary>
    /// <param name="services"></param>
    protected virtual void ConfigureTestServices(IServiceCollection services) { }

    protected HttpClient CreateClient(bool addDefaultAuthorizationHeader = true, TestUser testUser = TestUser.ReadWrite)
    {
        var builder = _factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureTestServices(ConfigureTestServices);
        });

        var client = builder.CreateClient();

        if (addDefaultAuthorizationHeader)
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                BasicAuthenticationHandler.SchemeName,
                Convert.ToBase64String(
                    testUser switch
                    {
                        TestUser.ReadOnly => "IntegrationTest-Read:integration-test-read"u8.ToArray(),
                        TestUser.WriteOnly => "IntegrationTest-Write:integration-test-write"u8.ToArray(),
                        _ => "IntegrationTest-ReadWrite:integration-test-readwrite"u8.ToArray(),
                    }
                )
            );

        return client;
    }

    protected enum TestUser
    {
        ReadWrite,
        ReadOnly,
        WriteOnly,
    }
}
