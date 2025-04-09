using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace Defra.TradeImportsDataApi.Api.Client;

public class TradeImportsDataApiClient(HttpClient httpClient, ILogger<TradeImportsDataApiClient> logger)
    : ITradeImportsDataApiClient
{
    private static readonly JsonSerializerOptions s_options = new();

    public async Task<ImportNotificationResponse?> GetImportNotification(
        string chedId,
        CancellationToken cancellationToken
    )
    {
        var requestUri = Endpoints.ImportNotifications(chedId);
        var message = new HttpRequestMessage(HttpMethod.Get, new Uri(requestUri, UriKind.RelativeOrAbsolute))
        {
            Version = httpClient.DefaultRequestVersion,
            VersionPolicy = HttpVersionPolicy.RequestVersionOrLower,
        };

        var response = await httpClient.SendAsync(message, HttpCompletionOption.ResponseHeadersRead, cancellationToken);

        if (response.StatusCode == HttpStatusCode.NotFound)
            return null;

        response.EnsureSuccessStatusCode();

        await using var stream = await response.Content.ReadAsStreamAsync(cancellationToken);

        var result =
            await JsonSerializer.DeserializeAsync<ImportNotificationResponse>(stream, s_options, cancellationToken)
            ?? throw new InvalidOperationException("Deserialized null");

        return result with
        {
            ETag = response.Headers.ETag?.Tag,
        };
    }

    public async Task PutImportNotification(
        string chedId,
        Domain.Ipaffs.ImportNotification data,
        string? etag,
        CancellationToken cancellationToken
    )
    {
        var requestUri = Endpoints.ImportNotifications(chedId);
        var message = new HttpRequestMessage(HttpMethod.Put, new Uri(requestUri, UriKind.RelativeOrAbsolute))
        {
            Version = httpClient.DefaultRequestVersion,
            VersionPolicy = HttpVersionPolicy.RequestVersionOrLower,
            Content = JsonContent.Create(data, options: s_options),
        };

        if (!string.IsNullOrEmpty(etag))
            message.Headers.IfMatch.Add(new EntityTagHeaderValue(etag));

        var response = await httpClient.SendAsync(message, HttpCompletionOption.ResponseHeadersRead, cancellationToken);

        await LogResponseForBadRequest(requestUri, response, cancellationToken);

        response.EnsureSuccessStatusCode();
    }

    private async Task LogResponseForBadRequest(
        string requestUri,
        HttpResponseMessage response,
        CancellationToken cancellationToken
    )
    {
        if (response.StatusCode == HttpStatusCode.BadRequest)
        {
            await using var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
            using var streamReader = new StreamReader(stream);
            var content = await streamReader.ReadToEndAsync(cancellationToken);

            logger.LogWarning("BadRequest {RequestUri} {Content}", requestUri, content);
        }
    }
}
