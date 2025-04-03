using System.Text.Json.Serialization;

namespace Defra.TradeImportsDataApi.Domain.Ipaffs;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum DecisionFreeCirculationPurpose
{
    AnimalFeedingStuff,

    HumanConsumption,

    PharmaceuticalUse,

    TechnicalUse,

    FurtherProcess,

    Other,
}
