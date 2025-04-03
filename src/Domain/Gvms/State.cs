using System.Text.Json.Serialization;

namespace Defra.TradeImportsData.Domain.Gvms;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum State
{
    NotFinalisable,

    Open,

    Finalised,

    CheckedIn,

    Embarked,

    Completed,
}
