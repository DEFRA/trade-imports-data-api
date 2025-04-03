using System.Text.Json.Serialization;

namespace Defra.TradeImportsData.Domain.CustomsDeclaration.ClearanceRequest;

public class Check
{
    [JsonPropertyName("checkCode")]
    public string? CheckCode { get; set; }

    [JsonPropertyName("departmentCode")]
    public string? DepartmentCode { get; set; }
}
