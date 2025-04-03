using System.Text.Json.Serialization;

namespace Defra.TradeImportsData.Domain.Gvms;

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
