using Defra.TradeImportsData.Domain.Json;
using System.Text.Json.Serialization;

namespace Defra.TradeImportsData.Domain.CustomsDeclaration.ClearanceRequest;

public class Document
{
    [JsonPropertyName("documentCode")]
    public string? DocumentCode { get; set; }

    [JsonPropertyName("documentReference")]
    [JsonConverter(typeof(DocumentReferenceJsonConverterAttribute))]
    public DocumentReference? DocumentReference { get; set; }

    [JsonPropertyName("documentStatus")]
    public string? DocumentStatus { get; set; }

    [JsonPropertyName("documentControl")]
    public string? DocumentControl { get; set; }

    [JsonPropertyName("documentQuantity")]
    public decimal? DocumentQuantity { get; set; }
}