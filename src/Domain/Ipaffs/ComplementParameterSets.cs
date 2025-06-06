using System.Text.Json.Serialization;

namespace Defra.TradeImportsDataApi.Domain.Ipaffs;

public class ComplementParameterSets
{
    /// <summary>
    ///     UUID used to match commodityComplement to its complementParameter set. CHEDPP only
    /// </summary>
    [JsonPropertyName("uniqueComplementId")]
    public string? UniqueComplementId { get; set; }

    [JsonPropertyName("complementId")]
    public int? ComplementId { get; set; }

    [JsonPropertyName("speciesId")]
    public string? SpeciesId { get; set; }

    [JsonPropertyName("keyDataPair")]
    public KeyDataPair[]? KeyDataPairs { get; set; }

    /// <summary>
    ///     Catch certificate details
    /// </summary>
    [JsonPropertyName("catchCertificates")]
    public CatchCertificates[]? CatchCertificates { get; set; }

    /// <summary>
    ///     Data used to identify the complements inside an IMP consignment
    /// </summary>
    [JsonPropertyName("identifiers")]
    public Identifiers[]? Identifiers { get; set; }
}
