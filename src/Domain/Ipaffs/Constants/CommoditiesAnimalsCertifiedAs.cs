namespace Defra.TradeImportsDataApi.Domain.Ipaffs.Constants;

public static class CommoditiesAnimalsCertifiedAs
{
    public const string AnimalFeedingStuff = "Animal feeding stuff";
    public const string Approved = "Approved";
    public const string ArtificialReproduction = "Artificial reproduction";
    public const string Breeding = "Breeding";
    public const string Circus = "Circus";
    public const string CommercialSale = "Commercial sale";
    public const string CommercialSaleOrChangeOfOwnership = "Commercial sale or change of ownership";
    public const string Fattening = "Fattening";
    public const string GameRestocking = "Game restocking";
    public const string HumanConsumption = "Human consumption";
    public const string InternalMarket = "Internal market";
    public const string Other = "Other";
    public const string PersonallyOwnedPetsNotForRehoming = "Personally owned pets not for rehoming";
    public const string Pets = "Pets";
    public const string Production = "Production";
    public const string Quarantine = "Quarantine";
    public const string RacingCompetition = "Racing/Competition";
    public const string RegisteredEquidae = "Registered equidae";
    public const string Registered = "Registered";
    public const string RejectedOrReturnedConsignment = "Rejected or Returned consignment";
    public const string Relaying = "Relaying";
    public const string RescueRehoming = "Rescue/Rehoming";
    public const string Research = "Research";
    public const string Slaughter = "Slaughter";
    public const string TechnicalPharmaceuticalUse = "Technical/Pharmaceutical use";
    public const string Transit = "Transit";
    public const string ZooCollection = "Zoo/Collection";

    public static bool IsAnimalFeedingStuff(string? status) => Equals(AnimalFeedingStuff, status);

    public static bool IsApproved(string? status) => Equals(Approved, status);

    public static bool IsArtificialReproduction(string? status) => Equals(ArtificialReproduction, status);

    public static bool IsBreeding(string? status) => Equals(Breeding, status);

    public static bool IsCircus(string? status) => Equals(Circus, status);

    public static bool IsCommercialSale(string? status) => Equals(CommercialSale, status);

    public static bool IsCommercialSaleOrChangeOfOwnership(string? status) =>
        Equals(CommercialSaleOrChangeOfOwnership, status);

    public static bool IsFattening(string? status) => Equals(Fattening, status);

    public static bool IsGameRestocking(string? status) => Equals(GameRestocking, status);

    public static bool IsHumanConsumption(string? status) => Equals(HumanConsumption, status);

    public static bool IsInternalMarket(string? status) => Equals(InternalMarket, status);

    public static bool IsOther(string? status) => Equals(Other, status);

    public static bool IsPersonallyOwnedPetsNotForRehoming(string? status) =>
        Equals(PersonallyOwnedPetsNotForRehoming, status);

    public static bool IsPets(string? status) => Equals(Pets, status);

    public static bool IsProduction(string? status) => Equals(Production, status);

    public static bool IsQuarantine(string? status) => Equals(Quarantine, status);

    public static bool IsRacingCompetition(string? status) => Equals(RacingCompetition, status);

    public static bool IsRegisteredEquidae(string? status) => Equals(RegisteredEquidae, status);

    public static bool IsRegistered(string? status) => Equals(Registered, status);

    public static bool IsRejectedOrReturnedConsignment(string? status) => Equals(RejectedOrReturnedConsignment, status);

    public static bool IsRelaying(string? status) => Equals(Relaying, status);

    public static bool IsRescueRehoming(string? status) => Equals(RescueRehoming, status);

    public static bool IsResearch(string? status) => Equals(Research, status);

    public static bool IsSlaughter(string? status) => Equals(Slaughter, status);

    public static bool IsTechnicalPharmaceuticalUse(string? status) => Equals(TechnicalPharmaceuticalUse, status);

    public static bool IsTransit(string? status) => Equals(Transit, status);

    public static bool IsZooCollection(string? status) => Equals(ZooCollection, status);

    private static bool Equals(string status1, string? status2) =>
        status1.Equals(status2, StringComparison.OrdinalIgnoreCase);
}
