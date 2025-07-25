using System.Linq.Expressions;
using Defra.TradeImportsDataApi.Data.Entities;

namespace Defra.TradeImportsDataApi.Api.Data;

public interface ICustomsDeclarationRepository
{
    Task<CustomsDeclarationEntity?> Get(string id, CancellationToken cancellationToken);

    Task<List<CustomsDeclarationEntity>> GetAll(
        string importPreNotificationIdentifier,
        CancellationToken cancellationToken
    );

    Task<List<CustomsDeclarationEntity>> GetAll(
        Expression<Func<CustomsDeclarationEntity, bool>> predicate,
        CancellationToken cancellationToken
    );

    Task<List<string>> GetAllIds(string importPreNotificationIdentifier, CancellationToken cancellationToken);

    Task<List<string>> GetAllImportPreNotificationIdentifiers(string id, CancellationToken cancellationToken);

    Task<List<string>> GetAllImportPreNotificationIdentifiers(string[] ids, CancellationToken cancellationToken);

    CustomsDeclarationEntity Insert(CustomsDeclarationEntity entity);

    Task<(CustomsDeclarationEntity Existing, CustomsDeclarationEntity Updated)> Update(
        CustomsDeclarationEntity entity,
        string etag,
        CancellationToken cancellationToken
    );
}
