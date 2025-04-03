using Defra.TradeImportsDataApi.Data.Entities;

namespace Defra.TradeImportsDataApi.Api.Services;

public interface IImportNotificationService
{
    Task<ImportNotificationEntity?> GetImportNotification(string chedId, CancellationToken cancellationToken);
    Task<ImportNotificationEntity> Insert(ImportNotificationEntity importNotificationEntity, CancellationToken cancellationToken);
    Task<ImportNotificationEntity> Update(ImportNotificationEntity importNotificationEntity, string etag, CancellationToken cancellationToken);
}