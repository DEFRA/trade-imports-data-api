using System.Text.Json.Serialization;

namespace Defra.TradeImportsDataApi.Domain.Ipaffs;

/// <summary>
/// Information about single laboratory test
/// </summary>
public class SingleLaboratoryTest
{
    /// <summary>
    /// Commodity code for which lab test was ordered
    /// </summary>
    [JsonPropertyName("commodityCode")]
    [System.ComponentModel.Description("Commodity code for which lab test was ordered")]
    public string? CommodityCode { get; set; }

    /// <summary>
    /// Species id of commodity for which lab test was ordered
    /// </summary>
    [JsonPropertyName("speciesId")]
    [System.ComponentModel.Description("Species id of commodity for which lab test was ordered")]
    public int? SpeciesId { get; set; }

    /// <summary>
    /// TRACES ID
    /// </summary>
    [JsonPropertyName("tracesId")]
    [System.ComponentModel.Description("TRACES ID")]
    public int? TracesId { get; set; }

    /// <summary>
    /// Test name
    /// </summary>
    [JsonPropertyName("testName")]
    [System.ComponentModel.Description("Test name")]
    public string? TestName { get; set; }

    /// <summary>
    /// Laboratory tests information details and information about laboratory
    /// </summary>
    [JsonPropertyName("applicant")]
    [System.ComponentModel.Description("Laboratory tests information details and information about laboratory")]
    public Applicant? Applicant { get; set; }

    /// <summary>
    /// Information about results of test
    /// </summary>
    [JsonPropertyName("laboratoryTestResult")]
    [System.ComponentModel.Description("Information about results of test")]
    public LaboratoryTestResult? LaboratoryTestResult { get; set; }
}
