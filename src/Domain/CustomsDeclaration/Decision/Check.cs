using System.Text.Json.Serialization;

namespace Defra.TradeImportsData.Domain.CustomsDeclaration.Decision;

public class Check
{
    [JsonPropertyName("checkCode")]
    public required string CheckCode { get; set; }

    [JsonPropertyName("departmentCode")]
    public string? DepartmentCode { get; set; }

    [JsonPropertyName("decisionCode")]
    public required string DecisionCode { get; set; }

    [JsonPropertyName("decisionsValidUntil")]
    public DateTime? DecisionsValidUntil { get; set; }

    [JsonPropertyName("decisionReasons")]
    public string[]? DecisionReasons { get; set; }

    [JsonPropertyName("decisionInternalFurtherDetail")]
    public string[]? DecisionInternalFurtherDetail { get; set; }
}
