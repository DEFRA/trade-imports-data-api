namespace Defra.TradeImportsDataApi.Domain.Ipaffs.Constants;

public static class DecisionDecision
{
    public const string NonAcceptable = "Non Acceptable";
    public const string AcceptableForInternalMarket = "Acceptable for Internal Market";
    public const string AcceptableIfChanneled = "Acceptable if Channeled";
    public const string AcceptableForTranshipment = "Acceptable for Transhipment";
    public const string AcceptableForTransit = "Acceptable for Transit";
    public const string AcceptableForTemporaryImport = "Acceptable for Temporary Import";
    public const string AcceptableForSpecificWarehouse = "Acceptable for Specific Warehouse";
    public const string AcceptableForPrivateImport = "Acceptable for Private Import";
    public const string AcceptableForTransfer = "Acceptable for Transfer";
    public const string HorseReEntry = "Horse Re-entry";

    public static bool IsNonAcceptable(string? status) => Equals(NonAcceptable, status);

    public static bool IsAcceptableForInternalMarket(string? status) => Equals(AcceptableForInternalMarket, status);

    public static bool IsAcceptableIfChanneled(string? status) => Equals(AcceptableIfChanneled, status);

    public static bool IsAcceptableForTranshipment(string? status) => Equals(AcceptableForTranshipment, status);

    public static bool IsAcceptableForTransit(string? status) => Equals(AcceptableForTransit, status);

    public static bool IsAcceptableForTemporaryImport(string? status) => Equals(AcceptableForTemporaryImport, status);

    public static bool IsAcceptableForSpecificWarehouse(string? status) =>
        Equals(AcceptableForSpecificWarehouse, status);

    public static bool IsAcceptableForPrivateImport(string? status) => Equals(AcceptableForPrivateImport, status);

    public static bool IsAcceptableForTransfer(string? status) => Equals(AcceptableForTransfer, status);

    public static bool IsHorseReEntry(string? status) => Equals(HorseReEntry, status);

    private static bool Equals(string status1, string? status2) =>
        status1.Equals(status2, StringComparison.OrdinalIgnoreCase);
}
