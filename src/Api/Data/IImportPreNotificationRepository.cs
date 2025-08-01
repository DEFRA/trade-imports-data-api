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
        ImportPreNotificationUpdateQuery query,
        CancellationToken cancellationToken = default
    );

    ImportPreNotificationEntity Insert(ImportPreNotificationEntity entity);

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

    void TrackImportPreNotificationUpdate(ImportPreNotificationEntity entity);

    Task<string?> GetMaxId(CancellationToken cancellationToken);
}
