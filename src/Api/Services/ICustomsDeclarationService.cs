using Defra.TradeImportsDataApi.Data.Entities;

namespace Defra.TradeImportsDataApi.Api.Services;

public interface ICustomsDeclarationService
{
    Task<CustomsDeclarationEntity?> GetCustomsDeclaration(string id, CancellationToken cancellationToken);

    Task<List<CustomsDeclarationEntity>> GetCustomsDeclarationsByChedId(
        string chedId,
        CancellationToken cancellationToken
    );

    Task<CustomsDeclarationEntity> Insert(CustomsDeclarationEntity entity, CancellationToken cancellationToken);

    Task<CustomsDeclarationEntity> Update(
        CustomsDeclarationEntity entity,
        string etag,
        CancellationToken cancellationToken
    );
}
