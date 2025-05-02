namespace Defra.TradeImportsDataApi.Domain.Ipaffs.Constants;

public static class DecisionNotAcceptableActionQuarantineImposedReason
{
    public const string ContaminatedProducts = "ContaminatedProducts";
    public const string InterceptedPart = "InterceptedPart";
    public const string PackagingMaterial = "PackagingMaterial";
    public const string Other = "Other";

    public static bool IsContaminatedProducts(string? status) => Equals(ContaminatedProducts, status);

    public static bool IsInterceptedPart(string? status) => Equals(InterceptedPart, status);

    public static bool IsPackagingMaterial(string? status) => Equals(PackagingMaterial, status);

    public static bool IsOther(string? status) => Equals(Other, status);

    private static bool Equals(string status1, string? status2) =>
        status1.Equals(status2, StringComparison.OrdinalIgnoreCase);
}
