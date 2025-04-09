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

    Task<GmrResponse?> GetGmr(string gmrId, CancellationToken cancellationToken);

    Task PutGmr(string gmrId, Domain.Gvms.Gmr data, string? etag, CancellationToken cancellationToken);
}
