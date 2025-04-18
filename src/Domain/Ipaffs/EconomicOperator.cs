using System.Text.Json.Serialization;

namespace Defra.TradeImportsDataApi.Domain.Ipaffs;

/// <summary>
/// An organisation as part of the DEFRA system
/// </summary>
public class EconomicOperator
{
    /// <summary>
    /// The unique identifier of this organisation
    /// </summary>

    [JsonPropertyName("id")]
    [System.ComponentModel.Description("The unique identifier of this organisation")]
    public string? Id { get; set; }

    /// <summary>
    /// Type of organisation
    /// </summary>

    [JsonPropertyName("type")]
    [System.ComponentModel.Description("Type of organisation")]
    public EconomicOperatorType? Type { get; set; }

    /// <summary>
    /// Status of organisation
    /// </summary>

    [JsonPropertyName("status")]
    [System.ComponentModel.Description("Status of organisation")]
    public EconomicOperatorStatus? Status { get; set; }

    /// <summary>
    /// Name of organisation
    /// </summary>

    [JsonPropertyName("companyName")]
    [System.ComponentModel.Description("Name of organisation")]
    public string? CompanyName { get; set; }

    /// <summary>
    /// Individual name
    /// </summary>

    [JsonPropertyName("individualName")]
    [System.ComponentModel.Description("Individual name")]
    public string? IndividualName { get; set; }

    /// <summary>
    /// Address of economic operator
    /// </summary>

    [JsonPropertyName("address")]
    [System.ComponentModel.Description("Address of economic operator")]
    public Address? Address { get; set; }

    /// <summary>
    /// Approval Number which identifies an Economic Operator unambiguously per type of organisation per country.
    /// </summary>

    [JsonPropertyName("approvalNumber")]
    [System.ComponentModel.Description(
        "Approval Number which identifies an Economic Operator unambiguously per type of organisation per country."
    )]
    public string? ApprovalNumber { get; set; }

    /// <summary>
    /// Optional Business General Number, often named Aggregation Code, which identifies an Economic Operator.
    /// </summary>

    [JsonPropertyName("otherIdentifier")]
    [System.ComponentModel.Description(
        "Optional Business General Number, often named Aggregation Code, which identifies an Economic Operator."
    )]
    public string? OtherIdentifier { get; set; }

    /// <summary>
    /// Traces Id of the economic operator generated by IPAFFS
    /// </summary>

    [JsonPropertyName("tracesId")]
    [System.ComponentModel.Description("Traces Id of the economic operator generated by IPAFFS")]
    public int? TracesId { get; set; }
}
