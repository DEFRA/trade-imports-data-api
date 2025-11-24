using System.Text.Json.Serialization;
using Defra.TradeImportsDataApi.Domain.CustomsDeclaration;

namespace Defra.TradeImportsDataApi.Domain.Events;

public class CustomsDeclarationEvent
{
    public required string Id { get; set; }

    public string Etag { get; set; } = null!;

    public DateTime Created { get; set; }

    public DateTime Updated { get; set; }

    public ClearanceRequest? ClearanceRequest { get; set; }

    public ClearanceDecision? ClearanceDecision { get; set; }

    public Finalisation? Finalisation { get; set; }

    public ExternalError[]? ExternalErrors { get; set; }
}
