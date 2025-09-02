using System.Text.Json.Serialization;

namespace Defra.TradeImportsDataApi.Api.Endpoints.Reporting;

public record ReportDataset(
    [property: JsonPropertyName("label")] string Label,
    [property: JsonPropertyName("data")] int[] Data
);
