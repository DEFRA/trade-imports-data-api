using System.Text.Json.Serialization;
using Defra.TradeImportsDataApi.Domain.Json;

namespace Defra.TradeImportsDataApi.Domain.CustomsDeclaration;

public class ImportDocument
{
    [JsonPropertyName("documentCode")]
    public string? DocumentCode { get; set; }

    [JsonPropertyName("documentReference")]
    [DocumentReferenceJsonConverter]
    public ImportDocumentReference? DocumentReference { get; set; }

    [JsonPropertyName("documentStatus")]
    public string? DocumentStatus { get; set; }

    [JsonPropertyName("documentControl")]
    public string? DocumentControl { get; set; }

    [JsonPropertyName("documentQuantity")]
    public decimal? DocumentQuantity { get; set; }

    public bool HasValidDocumentReference()
    {
        return !string.IsNullOrEmpty(DocumentCode) && ImportDocumentReference.IsValid(DocumentCode);
    }

    public string GetDocumentReferenceIdentifier()
    {
        if (!string.IsNullOrEmpty(DocumentCode) && DocumentReference != null)
            return DocumentReference.GetIdentifier(DocumentCode);

        return string.Empty;
    }
}
