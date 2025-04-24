using System.Text.Json.Serialization;

namespace Defra.TradeImportsDataApi.Domain.Ipaffs;

/// <summary>
/// Approved Establishment details
/// </summary>
public class ApprovedEstablishment
{
    /// <summary>
    /// ID
    /// </summary>
    [JsonPropertyName("id")]
    [System.ComponentModel.Description("ID")]
    public string? Id { get; set; }

    /// <summary>
    /// Name of approved establishment
    /// </summary>
    [JsonPropertyName("name")]
    [System.ComponentModel.Description("Name of approved establishment")]
    public string? Name { get; set; }

    /// <summary>
    /// Country of approved establishment
    /// </summary>
    [JsonPropertyName("country")]
    [System.ComponentModel.Description("Country of approved establishment")]
    public string? Country { get; set; }

    /// <summary>
    /// Types of approved establishment
    /// </summary>
    [JsonPropertyName("types")]
    [System.ComponentModel.Description("Types of approved establishment")]
    public string[]? Types { get; set; }

    /// <summary>
    /// Approval number
    /// </summary>
    [JsonPropertyName("approvalNumber")]
    [System.ComponentModel.Description("Approval number")]
    public string? ApprovalNumber { get; set; }

    /// <summary>
    /// Section of approved establishment
    /// </summary>
    [JsonPropertyName("section")]
    [System.ComponentModel.Description("Section of approved establishment")]
    public string? Section { get; set; }
}
