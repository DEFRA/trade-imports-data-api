using System.Text.Json.Serialization;
using Defra.TradeImportsData.Domain.Json;

namespace Defra.TradeImportsData.Domain.CustomsDeclaration.ClearanceRequest;

public class Document
{
    [JsonPropertyName("documentCode")]
    public string? DocumentCode { get; set; }

    [JsonPropertyName("documentReference")]
    [DocumentReferenceJsonConverter]
    public DocumentReference? DocumentReference { get; set; }

    [JsonPropertyName("documentStatus")]
    public string? DocumentStatus { get; set; }

    [JsonPropertyName("documentControl")]
    public string? DocumentControl { get; set; }

    [JsonPropertyName("documentQuantity")]
    public decimal? DocumentQuantity { get; set; }
}
