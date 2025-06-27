using System.Text.Json.Serialization;

namespace Defra.TradeImportsDataApi.Api.Endpoints.Admin;

public record MaxIdResponse([property: JsonPropertyName("importPreNotification")] string? ImportPreNotification);
