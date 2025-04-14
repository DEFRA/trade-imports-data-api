using System.Text.Json.Serialization;
using Defra.TradeImportsDataApi.Api.Endpoints.CustomsDeclarations;
using Defra.TradeImportsDataApi.Domain.Ipaffs;

namespace Defra.TradeImportsDataApi.Api.Endpoints.ImportPreNotifications;

public record ImportPreNotificationWithCustomDeclarationsResponse(
    [property: JsonPropertyName("importPreNotification")] ImportPreNotification ImportPreNotification,
    [property: JsonPropertyName("customsDeclarations")] List<CustomsDeclarationResponse> CustomsDeclarations,
    [property: JsonPropertyName("created")] DateTime Created,
    [property: JsonPropertyName("updated")] DateTime Updated
);