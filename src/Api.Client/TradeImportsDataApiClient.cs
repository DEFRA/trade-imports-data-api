using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using Defra.TradeImportsDataApi.Domain.CustomsDeclaration;
using Defra.TradeImportsDataApi.Domain.Errors;
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
            RequestId = response.Headers.TryGetValues("X-Request-ID", out var values) ? values.FirstOrDefault() : null,
        };
    }

    public async Task<CustomsDeclarationsResponse> GetCustomsDeclarationsByChedId(
        string chedId,
        CancellationToken cancellationToken
    )
    {
        var response = await Get(Endpoints.CustomsDeclarationsByChed(chedId), cancellationToken);

        response.EnsureSuccessStatusCode();

        return await Deserialize<CustomsDeclarationsResponse>(response, cancellationToken);
    }

    public async Task<GmrsResponse> GetGmrsByChedId(string chedId, CancellationToken cancellationToken)
    {
        var response = await Get(Endpoints.GmrsByChed(chedId), cancellationToken);

        response.EnsureSuccessStatusCode();

        return await Deserialize<GmrsResponse>(response, cancellationToken);
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

    public async Task<ImportPreNotificationsResponse> GetImportPreNotificationsByMrn(
        string mrn,
        CancellationToken cancellationToken
    )
    {
        var response = await Get(Endpoints.ImportPreNotificationsByMrn(mrn), cancellationToken);

        response.EnsureSuccessStatusCode();

        return await Deserialize<ImportPreNotificationsResponse>(response, cancellationToken);
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

    public async Task<ProcessingErrorResponse?> GetProcessingError(string mrn, CancellationToken cancellationToken)
    {
        var response = await Get(Endpoints.ProcessingErrors(mrn), cancellationToken);
        if (response.StatusCode == HttpStatusCode.NotFound)
            return null;

        response.EnsureSuccessStatusCode();

        var result = await Deserialize<ProcessingErrorResponse>(response, cancellationToken);

        return result with
        {
            ETag = response.Headers.ETag?.Tag,
        };
    }

    public async Task PutProcessingError(
        string mrn,
        ProcessingError[] data,
        string? etag,
        CancellationToken cancellationToken
    )
    {
        var requestUri = Endpoints.ProcessingErrors(mrn);
        var response = await Put(data, etag, requestUri, cancellationToken);

        response.EnsureSuccessStatusCode();
    }

    public async Task<RelatedImportDeclarationsResponse> RelatedImportDeclarations(
        RelatedImportDeclarationsRequest request,
        CancellationToken cancellationToken
    )
    {
        var response = await Get(Endpoints.RelatedImportDeclarations(request), cancellationToken);

        response.EnsureSuccessStatusCode();

        return await Deserialize<RelatedImportDeclarationsResponse>(response, cancellationToken);
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
