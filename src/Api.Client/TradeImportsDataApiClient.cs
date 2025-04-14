using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using Defra.TradeImportsDataApi.Domain.CustomsDeclaration;
using Defra.TradeImportsDataApi.Domain.Gvms;
using Defra.TradeImportsDataApi.Domain.Ipaffs;

namespace Defra.TradeImportsDataApi.Api.Client;

public class TradeImportsDataApiClient(HttpClient httpClient) : ITradeImportsDataApiClient
{
    private static readonly JsonSerializerOptions s_options = new();

    public async Task<ImportPreNotificationResponse?> GetImportPreNotification(
        string chedId,
        CancellationToken cancellationToken
    )
    {
        var response = await Get(Endpoints.ImportPreNotifications(chedId), cancellationToken);
        if (response.StatusCode == HttpStatusCode.NotFound)
            return null;

        response.EnsureSuccessStatusCode();

        var result = await Deserialize<ImportPreNotificationResponse>(response, cancellationToken);

        return result with
        {
            ETag = response.Headers.ETag?.Tag,
        };
    }

    public async Task<List<CustomsDeclarationResponse>?> GetCustomsDeclarationsByChedId(
        string chedId,
        CancellationToken cancellationToken
    )
    {
        var response = await Get(Endpoints.CustomsDeclarationsByChed(chedId), cancellationToken);
        if (response.StatusCode == HttpStatusCode.NotFound)
            return null;

        response.EnsureSuccessStatusCode();

        var result = await Deserialize<List<CustomsDeclarationResponse>?>(response, cancellationToken);

        return result;
    }

    public async Task PutImportPreNotification(
        string chedId,
        ImportPreNotification data,
        string? etag,
        CancellationToken cancellationToken
    )
    {
        var requestUri = Endpoints.ImportPreNotifications(chedId);
        var response = await Put(data, etag, requestUri, cancellationToken);

        response.EnsureSuccessStatusCode();
    }

    public async Task<GmrResponse?> GetGmr(string gmrId, CancellationToken cancellationToken)
    {
        var response = await Get(Endpoints.Gmrs(gmrId), cancellationToken);
        if (response.StatusCode == HttpStatusCode.NotFound)
            return null;

        response.EnsureSuccessStatusCode();

        var result = await Deserialize<GmrResponse>(response, cancellationToken);

        return result with
        {
            ETag = response.Headers.ETag?.Tag,
        };
    }

    public async Task PutGmr(string gmrId, Gmr data, string? etag, CancellationToken cancellationToken)
    {
        var requestUri = Endpoints.Gmrs(gmrId);
        var response = await Put(data, etag, requestUri, cancellationToken);

        response.EnsureSuccessStatusCode();
    }

    public async Task<CustomsDeclarationResponse?> GetCustomsDeclaration(
        string mrn,
        CancellationToken cancellationToken
    )
    {
        var response = await Get(Endpoints.CustomsDeclarations(mrn), cancellationToken);
        if (response.StatusCode == HttpStatusCode.NotFound)
            return null;

        response.EnsureSuccessStatusCode();

        var result = await Deserialize<CustomsDeclarationResponse>(response, cancellationToken);

        return result with
        {
            ETag = response.Headers.ETag?.Tag,
        };
    }

    public async Task<List<ImportPreNotificationResponse>?> GetImportPreNotificationsByMrn(
        string mrn,
        CancellationToken cancellationToken
    )
    {
        var response = await Get(Endpoints.ImportPreNotificationsByMrn(mrn), cancellationToken);
        if (response.StatusCode == HttpStatusCode.NotFound)
            return null;

        response.EnsureSuccessStatusCode();

        var result = await Deserialize<List<ImportPreNotificationResponse>?>(response, cancellationToken);

        return result;
    }

    public async Task PutCustomsDeclaration(
        string mrn,
        CustomsDeclaration data,
        string? etag,
        CancellationToken cancellationToken
    )
    {
        var requestUri = Endpoints.CustomsDeclarations(mrn);
        var response = await Put(data, etag, requestUri, cancellationToken);

        response.EnsureSuccessStatusCode();
    }

    private static async Task<T> Deserialize<T>(HttpResponseMessage response, CancellationToken cancellationToken)
    {
        await using var stream = await response.Content.ReadAsStreamAsync(cancellationToken);

        return await JsonSerializer.DeserializeAsync<T>(stream, s_options, cancellationToken)
            ?? throw new InvalidOperationException("Deserialized null");
    }

    private async Task<HttpResponseMessage> Get(string requestUri, CancellationToken cancellationToken)
    {
        var message = CreateMessage(HttpMethod.Get, requestUri);

        return await httpClient.SendAsync(message, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
    }

    private async Task<HttpResponseMessage> Put<T>(
        T data,
        string? etag,
        string requestUri,
        CancellationToken cancellationToken
    )
    {
        var message = CreateMessage(HttpMethod.Put, requestUri);
        message.Content = JsonContent.Create(data, options: s_options);

        if (!string.IsNullOrEmpty(etag))
            message.Headers.IfMatch.Add(new EntityTagHeaderValue(etag));

        return await httpClient.SendAsync(message, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
    }

    private HttpRequestMessage CreateMessage(HttpMethod method, string requestUri) =>
        new(method, new Uri(requestUri, UriKind.RelativeOrAbsolute))
        {
            Version = httpClient.DefaultRequestVersion,
            VersionPolicy = HttpVersionPolicy.RequestVersionOrLower,
        };
}
