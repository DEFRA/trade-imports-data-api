
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;


namespace Defra.TradeImportsData.Domain.IPaffs;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ImportNotificationTypeEnum
{

    Cveda,

    Cvedp,

    Chedpp,

    Ced,

    Imp,

}