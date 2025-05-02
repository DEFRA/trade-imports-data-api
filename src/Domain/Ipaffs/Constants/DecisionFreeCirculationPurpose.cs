namespace Defra.TradeImportsDataApi.Domain.Ipaffs.Constants;

public static class DecisionFreeCirculationPurpose
{
    public const string AnimalFeedingStuff = "Animal Feeding Stuff";
    public const string HumanConsumption = "Human Consumption";
    public const string PharmaceuticalUse = "Pharmaceutical Use";
    public const string TechnicalUse = "Technical Use";
    public const string FurtherProcess = "Further Process";
    public const string Other = "Other";

    public static bool IsAnimalFeedingStuff(string? status) => Equals(AnimalFeedingStuff, status);

    public static bool IsHumanConsumption(string? status) => Equals(HumanConsumption, status);

    public static bool IsPharmaceuticalUse(string? status) => Equals(PharmaceuticalUse, status);

    public static bool IsTechnicalUse(string? status) => Equals(TechnicalUse, status);

    public static bool IsFurtherProcess(string? status) => Equals(FurtherProcess, status);

    public static bool IsOther(string? status) => Equals(Other, status);

    private static bool Equals(string status1, string? status2) =>
        status1.Equals(status2, StringComparison.OrdinalIgnoreCase);
}
