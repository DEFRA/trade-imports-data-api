using System.Text.Json.Serialization;

namespace Defra.TradeImportsDataApi.Api.Client;

public record RelatedImportDeclarationsResponse(
    [property: JsonPropertyName("customsDeclarations")] CustomsDeclarationResponse[] CustomsDeclarations,
    [property: JsonPropertyName("importPreNotifications")] ImportPreNotificationResponse[] ImportPreNotifications,
    [property: JsonPropertyName("goodsVehicleMovements")] GmrResponse[] GoodsMovements,
    [property: JsonPropertyName("transitImportPreNotifications")]
        ImportPreNotificationResponse[] TransitImportPreNotifications,
    [property: JsonPropertyName("cheds")] TracesChedResponse[] Cheds
);
