using System.Text.Json.Serialization;

namespace Defra.TradeImportsDataApi.Domain.Ipaffs;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum PurposeInternalMarketPurpose
{
    AnimalFeedingStuff,

    HumanConsumption,

    PharmaceuticalUse,

    TechnicalUse,

    Other,

    CommercialSale,

    CommercialSaleOrChangeOfOwnership,

    Rescue,

    Breeding,

    Research,

    RacingOrCompetition,

    ApprovedPremisesOrBody,

    CompanionAnimalNotForResaleOrRehoming,

    Production,

    Slaughter,

    Fattening,

    GameRestocking,

    RegisteredHorses,
}
