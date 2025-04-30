using System.Text.Json.Serialization;
using Defra.TradeImportsDataApi.Domain.Json;

namespace Defra.TradeImportsDataApi.Domain.Ipaffs;

/// <summary>
/// Laboratory tests information details and information about laboratory that did the test
/// </summary>
public class Applicant
{
    /// <summary>
    /// Name of laboratory
    /// </summary>
    [JsonPropertyName("laboratory")]
    [System.ComponentModel.Description("Name of laboratory")]
    public string? Laboratory { get; set; }

    /// <summary>
    /// Laboratory address
    /// </summary>
    [JsonPropertyName("laboratoryAddress")]
    [System.ComponentModel.Description("Laboratory address")]
    public string? LaboratoryAddress { get; set; }

    /// <summary>
    /// Laboratory identification
    /// </summary>
    [JsonPropertyName("laboratoryIdentification")]
    [System.ComponentModel.Description("Laboratory identification")]
    public string? LaboratoryIdentification { get; set; }

    /// <summary>
    /// Laboratory phone number
    /// </summary>
    [JsonPropertyName("laboratoryPhoneNumber")]
    [System.ComponentModel.Description("Laboratory phone number")]
    public string? LaboratoryPhoneNumber { get; set; }

    /// <summary>
    /// Laboratory email
    /// </summary>
    [JsonPropertyName("laboratoryEmail")]
    [System.ComponentModel.Description("Laboratory email")]
    public string? LaboratoryEmail { get; set; }

    /// <summary>
    /// Sample batch number
    /// </summary>
    [JsonPropertyName("sampleBatchNumber")]
    [System.ComponentModel.Description("Sample batch number")]
    public string? SampleBatchNumber { get; set; }

    /// <summary>
    /// Type of analysis
    /// </summary>
    [JsonPropertyName("analysisType")]
    [System.ComponentModel.Description("Type of analysis")]
    public string? AnalysisType { get; set; }

    /// <summary>
    /// Number of samples analysed
    /// </summary>
    [JsonPropertyName("numberOfSamples")]
    [System.ComponentModel.Description("Number of samples analysed")]
    public int? NumberOfSamples { get; set; }

    /// <summary>
    /// Type of sample
    /// </summary>
    [JsonPropertyName("sampleType")]
    [System.ComponentModel.Description("Type of sample")]
    public string? SampleType { get; set; }

    /// <summary>
    /// Conservation of sample
    /// </summary>
    [JsonPropertyName("conservationOfSample")]
    [System.ComponentModel.Description("Conservation of sample")]
    public string? ConservationOfSample { get; set; }

    /// <summary>
    /// inspector
    /// </summary>
    [JsonPropertyName("inspector")]
    [System.ComponentModel.Description("inspector")]
    public Inspector? Inspector { get; set; }

    /// <summary>
    /// DateTime
    /// </summary>
    [JsonPropertyName("sampledOn")]
    [System.ComponentModel.Description("DateTime")]
    [UnknownTimeZoneDateTimeJsonConverter(nameof(SampledOn))]
    public DateTime? SampledOn { get; set; }
}
