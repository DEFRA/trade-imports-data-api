using System.Text.Json.Serialization;

namespace Defra.TradeImportsDataApi.Domain.CustomsDeclaration.ClearanceRequest;

public class ClearanceRequest
{
    [JsonPropertyName("correlationId")]
    public string? CorrelationId { get; set; }

    [JsonPropertyName("timestamp")]
    public DateTime Timestamp { get; set; }

    [JsonPropertyName("entryReference")]
    public string? EntryReference { get; set; }

    [JsonPropertyName("entryVersionNumber")]
    public int? EntryVersionNumber { get; set; }

    [JsonPropertyName("previousVersionNumber")]
    public int? PreviousVersionNumber { get; set; }

    [JsonPropertyName("declarationUcr")]
    public string? DeclarationUcr { get; set; }

    [JsonPropertyName("declarationPartNumber")]
    public string? DeclarationPartNumber { get; set; }

    [JsonPropertyName("declarationType")]
    public string? DeclarationType { get; set; }

    [JsonPropertyName("arrivesAt")]
    public DateTime? ArrivesAt { get; set; }

    [JsonPropertyName("submitterTurn")]
    public string? SubmitterTurn { get; set; }

    [JsonPropertyName("declarantId")]
    public string? DeclarantId { get; set; }

    [JsonPropertyName("declarantName")]
    public string? DeclarantName { get; set; }

    [JsonPropertyName("dispatchCountryCode")]
    public string? DispatchCountryCode { get; set; }

    [JsonPropertyName("goodsLocationCode")]
    public string? GoodsLocationCode { get; set; }

    [JsonPropertyName("masterUcr")]
    public string? MasterUcr { get; set; }

    [JsonPropertyName("items")]
    public Item[]? Items { get; set; }
}
