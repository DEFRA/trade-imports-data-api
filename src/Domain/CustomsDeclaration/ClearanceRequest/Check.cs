using System.Text.Json.Serialization;

namespace Defra.TradeImportsDataApi.Domain.CustomsDeclaration.ClearanceRequest;

public class Check
{
    [JsonPropertyName("checkCode")]
    public string? CheckCode { get; set; }

    [JsonPropertyName("departmentCode")]
    public string? DepartmentCode { get; set; }
}
