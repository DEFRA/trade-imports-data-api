namespace Defra.TradeImportsDataApi.Domain.CustomsDeclaration;

public enum FinalState
{
    // From https://eaflood.atlassian.net/wiki/spaces/ALVS/pages/5176590480/FinalState+Field
    Cleared = 0,
    CancelledAfterArrival = 1,
    CancelledWhilePreLodged = 2,
    Destroyed = 3,
    Seized = 4,
    ReleasedToKingsWarehouse = 5,
    TransferredToMss = 6,
}

public static class FinalStateExtensions
{
    public static bool IsCancelled(this FinalState finalState)
    {
        return finalState == FinalState.CancelledAfterArrival || finalState == FinalState.CancelledWhilePreLodged;
    }

    public static bool IsNotCancelled(this FinalState finalState)
    {
        return !finalState.IsCancelled();
    }
}
