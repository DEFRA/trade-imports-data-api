namespace Defra.TradeImportsDataApi.Domain.CustomsDeclaration;

public class CustomsDeclaration
{
    public ClearanceRequest? ClearanceRequest { get; set; }

    public ClearanceDecision? ClearanceDecision { get; set; }

    public Finalisation? Finalisation { get; set; }
}
