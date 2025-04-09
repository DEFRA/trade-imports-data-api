namespace Defra.TradeImportsDataApi.Api.Client;

public interface ITradeImportsDataApiClient
{
    Task<ImportNotificationResponse?> GetImportNotification(string chedId, CancellationToken cancellationToken);

    Task PutImportNotification(
        string chedId,
        Domain.Ipaffs.ImportPreNotification data,
        string? etag,
        CancellationToken cancellationToken
    );
}
