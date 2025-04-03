using System.ComponentModel;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Defra.TradeImportsData.Domain.IPaffs;

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
