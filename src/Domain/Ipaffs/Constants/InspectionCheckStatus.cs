namespace Defra.TradeImportsDataApi.Domain.Ipaffs.Constants;

public static class InspectionCheckStatus
{
    public const string Todo = "To do";
    public const string Compliant = "Compliant";
    public const string AutoCleared = "Auto cleared";
    public const string NonCompliant = "Non compliant";
    public const string NotInspected = "Not inspected";
    public const string ToBeInspected = "To be inspected";
    public const string Hold = "Hold";

    public static bool IsTodo(string? status) => Equals(Todo, status);

    public static bool IsCompliant(string? status) => Equals(Compliant, status);

    public static bool IsAutoCleared(string? status) => Equals(AutoCleared, status);

    public static bool IsNonCompliant(string? status) => Equals(NonCompliant, status);

    public static bool IsNotInspected(string? status) => Equals(NotInspected, status);

    public static bool IsToBeInspected(string? status) => Equals(ToBeInspected, status);

    public static bool IsHold(string? status) => Equals(Hold, status);

    private static bool Equals(string status1, string? status2) =>
        status1.Equals(status2, StringComparison.OrdinalIgnoreCase);
}
