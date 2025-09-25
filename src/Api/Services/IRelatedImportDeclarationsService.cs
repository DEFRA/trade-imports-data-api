using Defra.TradeImportsDataApi.Api.Endpoints.RelatedImportDeclarations;
using Defra.TradeImportsDataApi.Data.Entities;

namespace Defra.TradeImportsDataApi.Api.Services;

public interface IRelatedImportDeclarationsService
{
    Task<(
        CustomsDeclarationEntity[] CustomsDeclarations,
        ImportPreNotificationEntity[] ImportPreNotifications,
        GmrEntity[] Gmrs
    )> Search(RelatedImportDeclarationsRequest request, CancellationToken cancellationToken);
}
