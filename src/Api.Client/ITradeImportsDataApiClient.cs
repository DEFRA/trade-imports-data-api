namespace Defra.TradeImportsDataApi.Api.Client;

public interface ITradeImportsDataApiClient
{
    Task<ImportNotificationResponse?> GetImportNotification(string chedId, CancellationToken cancellationToken);

    Task<ImportNotificationResponse> PutImportNotification(
        string chedId,
        Domain.Ipaffs.ImportNotification data,
        string? etag,
        CancellationToken cancellationToken
    );
}
