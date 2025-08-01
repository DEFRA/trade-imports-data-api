using System.Net.Http.Json;

namespace Defra.TradeImportsDataApi.Testing;

public static class HttpClientExtensions
{
    public static async Task<T> GetFromJsonAsyncSafe<T>(this HttpClient httpClient, string requestUri)
    {
        var result = await httpClient.GetFromJsonAsync<T>(requestUri);

        if (result is null)
            throw new InvalidOperationException($"Unable to get JSON from {requestUri}");

        return result;
    }
}
