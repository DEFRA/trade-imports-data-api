using WireMock.Server;
using WireMock.Settings;
using WireMock.Types;

namespace Defra.TradeImportsDataApi.Testing;

#pragma warning disable S3881
public class WireMockContext : IDisposable
#pragma warning restore S3881
{
    public WireMockServer Server { get; }
    public HttpClient HttpClient { get; }

    // ReSharper disable once UnusedMember.Global
    public WireMockContext()
        : this(null) { }

    protected WireMockContext(QueryParameterMultipleValueSupport? queryParameterMultipleValueSupport)
    {
        Server = WireMockServer.Start(
            new WireMockServerSettings { QueryParameterMultipleValueSupport = queryParameterMultipleValueSupport }
        );
        HttpClient = new HttpClient { BaseAddress = new Uri(Server.Urls[0]) };
    }

#pragma warning disable CA1816
    public void Dispose()
#pragma warning restore CA1816
    {
        Server.Stop();
        Server.Dispose();
        HttpClient.Dispose();
    }
}

/// <summary>
/// This context turns off using comma as a delimiter within a query string.
/// </summary>
public class WireMockContextQueryParameterNoComma() : WireMockContext(QueryParameterMultipleValueSupport.NoComma);
