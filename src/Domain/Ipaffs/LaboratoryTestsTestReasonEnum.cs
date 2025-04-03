using System.Text.Json.Serialization;

namespace Defra.TradeImportsDataApi.Domain.Ipaffs;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum LaboratoryTestsTestReason
{
    Random,

    Suspicious,

    ReEnforced,

    IntensifiedControls,

    Required,

    LatentInfectionSampling,
}
