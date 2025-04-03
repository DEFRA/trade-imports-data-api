using System.Text.Json.Serialization;

namespace Defra.TradeImportsDataApi.Domain.Ipaffs;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ConsignmentDecision
{
    NonAcceptable,

    AcceptableForInternalMarket,

    AcceptableIfChanneled,

    AcceptableForTranshipment,

    AcceptableForTransit,

    AcceptableForTemporaryImport,

    AcceptableForSpecificWarehouse,

    AcceptableForPrivateImport,

    AcceptableForTransfer,

    HorseReEntry,
}
