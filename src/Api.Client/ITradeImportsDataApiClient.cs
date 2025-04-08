namespace Defra.TradeImportsDataApi.Api.Client;

public interface ITradeImportsDataApiClient
{
    Task<ClientResponse<ImportNotification>?> GetImportNotification(string chedId, CancellationToken cancellationToken);

    Task<ClientResponse<ImportNotification>?> PutImportNotification(
        string chedId,
        Domain.Ipaffs.ImportNotification data,
        string? etag,
        CancellationToken cancellationToken
    );
}
