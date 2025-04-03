using System.Text.Json.Serialization;

namespace Defra.TradeImportsData.Domain.IPaffs;

/// <summary>
/// Tests results corresponding to LaboratoryTests
/// </summary>
public class LaboratoryTestResult
{
    /// <summary>
    /// When sample was used
    /// </summary>

    [JsonPropertyName("sampleUseByDate")]
    [System.ComponentModel.Description("When sample was used")]
    public string? SampleUseByDate { get; set; }

    /// <summary>
    /// When it was released
    /// </summary>

    [JsonPropertyName("releasedOn")]
    [System.ComponentModel.Description("When it was released")]
    public DateOnly? ReleasedOn { get; set; }

    /// <summary>
    /// Laboratory test method
    /// </summary>

    [JsonPropertyName("laboratoryTestMethod")]
    [System.ComponentModel.Description("Laboratory test method")]
    public string? LaboratoryTestMethod { get; set; }

    /// <summary>
    /// Result of test
    /// </summary>

    [JsonPropertyName("results")]
    [System.ComponentModel.Description("Result of test")]
    public string? Results { get; set; }

    /// <summary>
    /// Conclusion of laboratory test
    /// </summary>

    [JsonPropertyName("conclusion")]
    [System.ComponentModel.Description("Conclusion of laboratory test")]
    public LaboratoryTestResultConclusion? Conclusion { get; set; }

    /// <summary>
    /// Date of lab test created in IPAFFS
    /// </summary>

    [JsonPropertyName("labTestCreatedOn")]
    [System.ComponentModel.Description("Date of lab test created in IPAFFS")]
    public DateOnly? LabTestCreatedOn { get; set; }
}
