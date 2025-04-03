namespace Defra.TradeImportsData.Domain.CustomsDeclaration
{
    public class CustomsDeclaration
    {
        public ClearanceRequest.ClearanceRequest? ClearanceRequest { get; set; }

        public Decision.Decision? Decision { get; set; }

        public Finalisation.Finalisation? Finalisation { get; set; }
    }
}
