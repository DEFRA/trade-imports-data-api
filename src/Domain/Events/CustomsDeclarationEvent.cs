using System.Text.Json.Serialization;
using Defra.TradeImportsDataApi.Domain.CustomsDeclaration;

namespace Defra.TradeImportsDataApi.Domain.Events;

public class CustomsDeclarationEvent
{
    [JsonPropertyName("id")]
    public required string Id { get; set; }

    [JsonPropertyName("etag")]
    public string ETag { get; set; } = null!;

    [JsonPropertyName("created")]
    public DateTime Created { get; set; }

    [JsonPropertyName("updated")]
    public DateTime Updated { get; set; }

    [JsonPropertyName("clearanceRequest")]
    public ClearanceRequest? ClearanceRequest { get; set; }

    [JsonPropertyName("clearanceDecision")]
    public ClearanceDecision? ClearanceDecision { get; set; }

    [JsonPropertyName("finalisation")]
    public Finalisation? Finalisation { get; set; }

    [JsonPropertyName("externalErrors")]
    public ExternalError[]? ExternalErrors { get; set; }
}
