namespace Defra.TradeImportsDataApi.Domain.Ipaffs.Constants;

public static class PurposeInternalMarketPurpose
{
    public const string AnimalFeedingStuff = "Animal Feeding Stuff";
    public const string HumanConsumption = "Human Consumption";
    public const string PharmaceuticalUse = "Pharmaceutical Use";
    public const string TechnicalUse = "Technical Use";
    public const string Other = "Other";
    public const string CommercialSale = "Commercial Sale";
    public const string CommercialSaleOrChangeOfOwnership = "Commercial sale or change of ownership";
    public const string Rescue = "Rescue";
    public const string Breeding = "Breeding";
    public const string Research = "Research";
    public const string RacingOrCompetition = "Racing or Competition";
    public const string ApprovedPremisesOrBody = "Approved Premises or Body";
    public const string CompanionAnimalNotForResaleOrRehoming = "Companion Animal not for Resale or Rehoming";
    public const string Production = "Production";
    public const string Slaughter = "Slaughter";
    public const string Fattening = "Fattening";
    public const string GameRestocking = "Game Restocking";
    public const string RegisteredHorses = "Registered Horses";

    public static bool IsAnimalFeedingStuff(string? status) => Equals(AnimalFeedingStuff, status);

    public static bool IsHumanConsumption(string? status) => Equals(HumanConsumption, status);

    public static bool IsPharmaceuticalUse(string? status) => Equals(PharmaceuticalUse, status);

    public static bool IsTechnicalUse(string? status) => Equals(TechnicalUse, status);

    public static bool IsOther(string? status) => Equals(Other, status);

    public static bool IsCommercialSale(string? status) => Equals(CommercialSale, status);

    public static bool IsCommercialSaleOrChangeOfOwnership(string? status) =>
        Equals(CommercialSaleOrChangeOfOwnership, status);

    public static bool IsRescue(string? status) => Equals(Rescue, status);

    public static bool IsBreeding(string? status) => Equals(Breeding, status);

    public static bool IsResearch(string? status) => Equals(Research, status);

    public static bool IsRacingOrCompetition(string? status) => Equals(RacingOrCompetition, status);

    public static bool IsApprovedPremisesOrBody(string? status) => Equals(ApprovedPremisesOrBody, status);

    public static bool IsCompanionAnimalNotForResaleOrRehoming(string? status) =>
        Equals(CompanionAnimalNotForResaleOrRehoming, status);

    public static bool IsProduction(string? status) => Equals(Production, status);

    public static bool IsSlaughter(string? status) => Equals(Slaughter, status);

    public static bool IsFattening(string? status) => Equals(Fattening, status);

    public static bool IsGameRestocking(string? status) => Equals(GameRestocking, status);

    public static bool IsRegisteredHorses(string? status) => Equals(RegisteredHorses, status);

    private static bool Equals(string status1, string? status2) =>
        status1.Equals(status2, StringComparison.OrdinalIgnoreCase);
}
