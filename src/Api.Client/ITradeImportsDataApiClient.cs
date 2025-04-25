namespace Defra.TradeImportsDataApi.Api.Client;

public interface ITradeImportsDataApiClient
{
    Task<ImportPreNotificationResponse?> GetImportPreNotification(string chedId, CancellationToken cancellationToken);

    Task<List<CustomsDeclarationResponse>?> GetCustomsDeclarationsByChedId(
        string chedId,
        CancellationToken cancellationToken
    );

    Task PutImportPreNotification(
        string chedId,
        Domain.Ipaffs.ImportPreNotification data,
        string? etag,
        CancellationToken cancellationToken
    );

    Task<GmrResponse?> GetGmr(string gmrId, CancellationToken cancellationToken);

    Task PutGmr(string gmrId, Domain.Gvms.Gmr data, string? etag, CancellationToken cancellationToken);

    Task<CustomsDeclarationResponse?> GetCustomsDeclaration(string mrn, CancellationToken cancellationToken);

    Task<List<ImportPreNotificationResponse>?> GetImportPreNotificationsByMrn(
        string mrn,
        CancellationToken cancellationToken
    );

    Task PutCustomsDeclaration(
        string mrn,
        Domain.CustomsDeclaration.CustomsDeclaration data,
        string? etag,
        CancellationToken cancellationToken
    );

    Task<ProcessingErrorResponse?> GetProcessingError(string mrn, CancellationToken cancellationToken);

    Task PutProcessingError(
        string mrn,
        Domain.ProcessingErrors.ProcessingError data,
        string? etag,
        CancellationToken cancellationToken
    );
}
