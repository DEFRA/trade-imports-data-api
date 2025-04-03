using System.Text.Json.Serialization;

namespace Defra.TradeImportsDataApi.Domain.Ipaffs;

/// <summary>
/// consignment checks
/// </summary>
public class ConsignmentCheck
{
    /// <summary>
    /// Does it conform EU standards
    /// </summary>

    [JsonPropertyName("euStandard")]
    [System.ComponentModel.Description("Does it conform EU standards")]
    public string? EuStandard { get; set; }

    /// <summary>
    /// Result of additional guarantees
    /// </summary>

    [JsonPropertyName("additionalGuarantees")]
    [System.ComponentModel.Description("Result of additional guarantees")]
    public string? AdditionalGuarantees { get; set; }

    /// <summary>
    /// Result of document check
    /// </summary>

    [JsonPropertyName("documentCheckResult")]
    [System.ComponentModel.Description("Result of document check")]
    public string? DocumentCheckResult { get; set; }

    /// <summary>
    /// Result of national requirements check
    /// </summary>

    [JsonPropertyName("nationalRequirements")]
    [System.ComponentModel.Description("Result of national requirements check")]
    public string? NationalRequirements { get; set; }

    /// <summary>
    /// Was identity check done
    /// </summary>

    [JsonPropertyName("identityCheckDone")]
    [System.ComponentModel.Description("Was identity check done")]
    public bool? IdentityCheckDone { get; set; }

    /// <summary>
    /// Type of identity check performed
    /// </summary>

    [JsonPropertyName("identityCheckType")]
    [System.ComponentModel.Description("Type of identity check performed")]
    public ConsignmentCheckIdentityCheckType? IdentityCheckType { get; set; }

    /// <summary>
    /// Result of identity check
    /// </summary>

    [JsonPropertyName("identityCheckResult")]
    [System.ComponentModel.Description("Result of identity check")]
    public string? IdentityCheckResult { get; set; }

    /// <summary>
    /// What was the reason for skipping identity check
    /// </summary>

    [JsonPropertyName("identityCheckNotDoneReason")]
    [System.ComponentModel.Description("What was the reason for skipping identity check")]
    public ConsignmentCheckIdentityCheckNotDoneReason? IdentityCheckNotDoneReason { get; set; }

    /// <summary>
    /// Was physical check done
    /// </summary>

    [JsonPropertyName("physicalCheckDone")]
    [System.ComponentModel.Description("Was physical check done")]
    public bool? PhysicalCheckDone { get; set; }

    /// <summary>
    /// Result of physical check
    /// </summary>

    [JsonPropertyName("physicalCheckResult")]
    [System.ComponentModel.Description("Result of physical check")]
    public string? PhysicalCheckResult { get; set; }

    /// <summary>
    /// What was the reason for skipping physical check
    /// </summary>

    [JsonPropertyName("physicalCheckNotDoneReason")]
    [System.ComponentModel.Description("What was the reason for skipping physical check")]
    public ConsignmentCheckPhysicalCheckNotDoneReason? PhysicalCheckNotDoneReason { get; set; }

    /// <summary>
    /// Other reason to not do physical check
    /// </summary>

    [JsonPropertyName("physicalCheckOtherText")]
    [System.ComponentModel.Description("Other reason to not do physical check")]
    public string? PhysicalCheckOtherText { get; set; }

    /// <summary>
    /// Welfare check
    /// </summary>

    [JsonPropertyName("welfareCheck")]
    [System.ComponentModel.Description("Welfare check")]
    public string? WelfareCheck { get; set; }

    /// <summary>
    /// Number of animals checked
    /// </summary>

    [JsonPropertyName("numberOfAnimalsChecked")]
    [System.ComponentModel.Description("Number of animals checked")]
    public int? NumberOfAnimalsChecked { get; set; }

    /// <summary>
    /// Were laboratory tests done
    /// </summary>

    [JsonPropertyName("laboratoryCheckDone")]
    [System.ComponentModel.Description("Were laboratory tests done")]
    public bool? LaboratoryCheckDone { get; set; }

    /// <summary>
    /// Result of laboratory tests
    /// </summary>

    [JsonPropertyName("laboratoryCheckResult")]
    [System.ComponentModel.Description("Result of laboratory tests")]
    public string? LaboratoryCheckResult { get; set; }
}
