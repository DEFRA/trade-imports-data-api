using System.Text.Json.Serialization;
using Defra.TradeImportsDataApi.Api.Endpoints.CustomsDeclarations;
using Defra.TradeImportsDataApi.Api.Endpoints.ImportPreNotifications;

namespace Defra.TradeImportsDataApi.Api.Endpoints.Search;

public record RelatedImportDeclarationsResponse(
    [property: JsonPropertyName("customsDeclarations")] CustomsDeclarationResponse[] CustomsDeclarations,
    [property: JsonPropertyName("importPreNotifications")] ImportPreNotificationResponse[] ImportPreNotifications
);
