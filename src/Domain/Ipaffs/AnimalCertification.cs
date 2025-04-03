using System.Text.Json.Serialization;

namespace Defra.TradeImportsData.Domain.IPaffs;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum AnimalCertification
{
    AnimalFeedingStuff,

    Approved,

    ArtificialReproduction,

    Breeding,

    Circus,

    CommercialSale,

    CommercialSaleOrChangeOfOwnership,

    Fattening,

    GameRestocking,

    HumanConsumption,

    InternalMarket,

    Other,

    PersonallyOwnedPetsNotForRehoming,

    Pets,

    Production,

    Quarantine,

    RacingCompetition,

    RegisteredEquidae,

    Registered,

    RejectedOrReturnedConsignment,

    Relaying,

    RescueRehoming,

    Research,

    Slaughter,

    TechnicalPharmaceuticalUse,

    Transit,

    ZooCollection,
}
