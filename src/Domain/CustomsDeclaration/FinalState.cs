namespace Defra.TradeImportsDataApi.Domain.CustomsDeclaration;

public static class FinalState
{
    public const string Cleared = "0";
    public const string CancelledAfterArrival = "1";
    public const string CancelledWhilePreLodged = "2";
    public const string Destroyed = "3";
    public const string Seized = "4";
    public const string ReleasedToKingsWarehouse = "5";
}
