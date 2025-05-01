using System.Text.Json.Serialization;
using Defra.TradeImportsDataApi.Domain.Attributes;
using Defra.TradeImportsDataApi.Domain.Json;

namespace Defra.TradeImportsDataApi.Domain.Ipaffs;

/// <summary>
/// Laboratory tests details
/// </summary>
public class LaboratoryTests
{
    /// <summary>
    /// Date of tests
    /// </summary>
    [JsonPropertyName("testedOn")]
    [System.ComponentModel.Description("Date of tests")]
    [UnknownTimeZoneDateTimeJsonConverter(nameof(TestedOn))]
    public DateTime? TestedOn { get; set; }

    /// <summary>
    /// Reason for test
    /// </summary>
    [JsonPropertyName("testReason")]
    [System.ComponentModel.Description("Reason for test")]
    [PossibleValue("Random")]
    [PossibleValue("Suspicious")]
    [PossibleValue("Re-enforced")]
    [PossibleValue("Intensified controls")]
    [PossibleValue("Required")]
    [PossibleValue("Latent infection sampling")]
    public string? TestReason { get; set; }

    /// <summary>
    /// List of details of individual tests performed or to be performed
    /// </summary>
    [JsonPropertyName("singleLaboratoryTests")]
    [System.ComponentModel.Description("List of details of individual tests performed or to be performed")]
    public SingleLaboratoryTest[]? SingleLaboratoryTests { get; set; }
}
