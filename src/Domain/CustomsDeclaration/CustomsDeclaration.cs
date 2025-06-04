namespace Defra.TradeImportsDataApi.Domain.CustomsDeclaration;

// Consider using this in the change set instead of the nested class
public class CustomsDeclaration
{
    public ClearanceRequest? ClearanceRequest { get; set; }

    public ClearanceDecision? ClearanceDecision { get; set; }

    public Finalisation? Finalisation { get; set; }

    public ExternalError[]? ExternalErrors { get; set; }
}
