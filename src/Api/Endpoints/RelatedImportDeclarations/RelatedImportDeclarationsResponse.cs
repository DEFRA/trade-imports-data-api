using System.Text.Json.Serialization;
using Defra.TradeImportsDataApi.Api.Endpoints.CustomsDeclarations;
using Defra.TradeImportsDataApi.Api.Endpoints.Gmrs;
using Defra.TradeImportsDataApi.Api.Endpoints.ImportPreNotifications;
using Defra.TradeImportsDataApi.Api.Endpoints.TracesChed;

namespace Defra.TradeImportsDataApi.Api.Endpoints.RelatedImportDeclarations;

public record RelatedImportDeclarationsResponse(
    [property: JsonPropertyName("customsDeclarations")] CustomsDeclarationResponse[] CustomsDeclarations,
    [property: JsonPropertyName("importPreNotifications")] ImportPreNotificationResponse[] ImportPreNotifications,
    [property: JsonPropertyName("goodsVehicleMovements")] GmrResponse[] GoodsMovements,
    [property: JsonPropertyName("transitImportPreNotifications")]
        ImportPreNotificationResponse[] TransitImportPreNotifications,
    [property: JsonPropertyName("cheds")] TracesChedResponse[] Cheds
);
