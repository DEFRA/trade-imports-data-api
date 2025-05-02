namespace Defra.TradeImportsDataApi.Domain.Ipaffs.Constants;

public static class DecisionNotAcceptableActionQuarantineImposedReason
{
    public const string Contaminatedproducts = "ContaminatedProducts";
    public const string Interceptedpart = "InterceptedPart";
    public const string Packagingmaterial = "PackagingMaterial";
    public const string Other = "Other";

    public static bool IsContaminatedproducts(string? status) => Equals(Contaminatedproducts, status);

    public static bool IsInterceptedpart(string? status) => Equals(Interceptedpart, status);

    public static bool IsPackagingmaterial(string? status) => Equals(Packagingmaterial, status);

    public static bool IsOther(string? status) => Equals(Other, status);

    private static bool Equals(string status1, string? status2) =>
        status1.Equals(status2, StringComparison.OrdinalIgnoreCase);
}
