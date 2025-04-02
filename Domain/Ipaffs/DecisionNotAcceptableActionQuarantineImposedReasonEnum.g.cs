
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;


namespace Defra.TradeImportsData.Domain.IPaffs;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum DecisionNotAcceptableActionQuarantineImposedReasonEnum
{

    ContaminatedProducts,

    InterceptedPart,

    PackagingMaterial,

    Other,

}