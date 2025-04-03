using System.Text.Json.Serialization;

namespace Defra.TradeImportsDataApi.Domain.Ipaffs;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum PurposePurposeGroup
{
    ForImport,

    ForNONConformingConsignments,

    ForTranshipmentTo,

    ForTransitTo3rdCountry,

    ForReImport,

    ForPrivateImport,

    ForTransferTo,

    ForImportReConformityCheck,

    ForImportNonInternalMarket,
}
