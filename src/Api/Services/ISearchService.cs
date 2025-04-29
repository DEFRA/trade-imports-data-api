using Defra.TradeImportsDataApi.Api.Endpoints.Search;
using Defra.TradeImportsDataApi.Data.Entities;

namespace Defra.TradeImportsDataApi.Api.Services;

public interface ISearchService
{
    Task<(CustomsDeclarationEntity[] CustomsDeclaration, ImportPreNotificationEntity[] ImportPreNotifications)> Search(
        SearchRequest searchRequest,
        CancellationToken cancellationToken
    );
}
