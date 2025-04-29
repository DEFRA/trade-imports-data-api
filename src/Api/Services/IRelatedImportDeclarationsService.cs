using Defra.TradeImportsDataApi.Api.Endpoints.Search;
using Defra.TradeImportsDataApi.Data.Entities;

namespace Defra.TradeImportsDataApi.Api.Services;

public interface IRelatedImportDeclarationsService
{
    Task<(CustomsDeclarationEntity[] CustomsDeclaration, ImportPreNotificationEntity[] ImportPreNotifications)> Search(
        RelatedImportDeclarationsRequest request,
        CancellationToken cancellationToken
    );
}
