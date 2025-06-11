using System.Linq.Expressions;
using Defra.TradeImportsDataApi.Data.Entities;

namespace Defra.TradeImportsDataApi.Api.Data;

public interface IImportPreNotificationRepository
{
    Task<ImportPreNotificationEntity?> Get(string id, CancellationToken cancellationToken);

    Task<ImportPreNotificationEntity?> GetByCustomsDeclarationIdentifier(
        string customsDeclarationIdentifier,
        CancellationToken cancellationToken
    );

    Task<List<ImportPreNotificationEntity>> GetAll(
        string[] customsDeclarationIdentifiers,
        CancellationToken cancellationToken
    );

    Task<List<ImportPreNotificationEntity>> GetAll(
        Expression<Func<ImportPreNotificationEntity, bool>> predicate,
        CancellationToken cancellationToken
    );

    Task<string?> GetCustomsDeclarationIdentifier(string id, CancellationToken cancellationToken);

    Task<ImportPreNotificationUpdates> GetUpdates(
        DateTime from,
        DateTime to,
        string[]? pointOfEntry = null,
        string[]? type = null,
        string[]? status = null,
        string[]? excludeStatus = null,
        int page = 1,
        int pageSize = 100,
        CancellationToken cancellationToken = default
    );

    Task<ImportPreNotificationEntity> Insert(ImportPreNotificationEntity entity, CancellationToken cancellationToken);

    Task<(ImportPreNotificationEntity Existing, ImportPreNotificationEntity Updated)> Update(
        ImportPreNotificationEntity entity,
        string etag,
        CancellationToken cancellationToken
    );

    Task TrackImportPreNotificationUpdate(
        IDataEntity source,
        string[] customsDeclarationIdentifiers,
        CancellationToken cancellationToken
    );

    Task TrackImportPreNotificationUpdate(ImportPreNotificationEntity entity, CancellationToken cancellationToken);
}
