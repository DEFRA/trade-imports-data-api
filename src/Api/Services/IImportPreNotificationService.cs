using Defra.TradeImportsDataApi.Api.Data;
using Defra.TradeImportsDataApi.Data.Entities;

namespace Defra.TradeImportsDataApi.Api.Services;

public interface IImportPreNotificationService
{
    Task<ImportPreNotificationEntity?> GetImportPreNotification(string chedId, CancellationToken cancellationToken);

    Task<List<ImportPreNotificationEntity>> GetImportPreNotificationsByMrn(
        string mrn,
        CancellationToken cancellationToken
    );

    Task<ImportPreNotificationEntity> Insert(ImportPreNotificationEntity entity, CancellationToken cancellationToken);

    Task<ImportPreNotificationEntity> Update(
        ImportPreNotificationEntity entity,
        string etag,
        CancellationToken cancellationToken
    );

    Task<ImportPreNotificationUpdates> GetImportPreNotificationUpdates(
        ImportPreNotificationUpdateQuery query,
        CancellationToken cancellationToken
    );
}
