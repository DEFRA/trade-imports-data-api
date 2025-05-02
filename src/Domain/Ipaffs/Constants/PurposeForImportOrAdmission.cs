namespace Defra.TradeImportsDataApi.Domain.Ipaffs.Constants;

public static class PurposeForImportOrAdmission
{
    public const string DefinitiveImport = "Definitive import";
    public const string HorsesReEntry = "Horses Re-entry";
    public const string TemporaryAdmissionHorses = "Temporary admission horses";

    public static bool IsDefinitiveImport(string? status) => Equals(DefinitiveImport, status);

    public static bool IsHorsesReEntry(string? status) => Equals(HorsesReEntry, status);

    public static bool IsTemporaryAdmissionHorses(string? status) => Equals(TemporaryAdmissionHorses, status);

    private static bool Equals(string status1, string? status2) =>
        status1.Equals(status2, StringComparison.OrdinalIgnoreCase);
}
