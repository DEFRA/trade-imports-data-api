using System.Text.Json.Serialization;

namespace Defra.TradeImportsDataApi.Api.Endpoints.Reporting;

public record ReportResponse(
    [property: JsonPropertyName("labels")] string[] Labels,
    [property: JsonPropertyName("datasets")] ReportDataset[] Datasets
);
