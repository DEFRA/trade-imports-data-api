namespace Defra.TradeImportsDataApi.Api.Client;

public interface ITradeImportsDataApiClient
{
    Task<ImportPreNotificationResponse?> GetImportPreNotification(string chedId, CancellationToken cancellationToken);

    Task PutImportPreNotification(
        string chedId,
        Domain.Ipaffs.ImportPreNotification data,
        string? etag,
        CancellationToken cancellationToken
    );
}
