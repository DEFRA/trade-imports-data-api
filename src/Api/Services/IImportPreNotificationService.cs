using Defra.TradeImportsDataApi.Data.Entities;

namespace Defra.TradeImportsDataApi.Api.Services;

public interface IImportPreNotificationService
{
    Task<ImportPreNotificationEntity?> GetImportPreNotification(string chedId, CancellationToken cancellationToken);
    Task<ImportPreNotificationEntity> Insert(
        ImportPreNotificationEntity importPreNotificationEntity,
        CancellationToken cancellationToken
    );
    Task<ImportPreNotificationEntity> Update(
        ImportPreNotificationEntity importPreNotificationEntity,
        string etag,
        CancellationToken cancellationToken
    );
}
