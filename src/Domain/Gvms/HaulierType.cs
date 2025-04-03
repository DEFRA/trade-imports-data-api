using System.Text.Json.Serialization;

namespace Defra.TradeImportsDataApi.Domain.Gvms;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum HaulierType
{
    Standard,

    FpoAsn,

    FpoOther,

    NatoMod,

    Rmg,

    Etoe,
}
