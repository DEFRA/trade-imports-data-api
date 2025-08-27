using System.Text.Json.Serialization;

namespace Defra.TradeImportsDataApi.Api.Endpoints.Reporting;

public record ManualReleaseReportResponse(
    [property: JsonPropertyName("total")] int Total,
    [property: JsonPropertyName("automaticRelease")] int AutomaticRelease,
    [property: JsonPropertyName("manualRelease")] int ManualRelease,
    [property: JsonPropertyName("manualReleaseMrns")] string[] ManualReleaseMrns
);
