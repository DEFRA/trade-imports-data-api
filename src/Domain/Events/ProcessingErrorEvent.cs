using System.Text.Json.Serialization;
using Defra.TradeImportsDataApi.Domain.Errors;

namespace Defra.TradeImportsDataApi.Domain.Events;

public class ProcessingErrorEvent
{
    public required string Id { get; set; }

    public string Etag { get; set; } = null!;

    public DateTime Created { get; set; }

    public DateTime Updated { get; set; }

    public required ProcessingError[] ProcessingErrors { get; set; }
}
