using System.Runtime.Serialization;

namespace Defra.TradeImportsData.Domain.IPaffs;


/// <summary>
/// This is currently separate to the Movement link status as we have the ched references on movement and so a better understanding
/// of what links should be present etc.
///
/// Should be called LinkStatus but causes a collision in OpenAPI spec so have had to give it a different name...
/// </summary>
////[JsonConverter(typeof(JsonStringEnumConverterEx<LinkStatus>))]
public enum LinkStatus
{
    [EnumMember(Value = "No Links")]
    NotLinked,

    [EnumMember(Value = "Linked")]
    Linked
}

////[JsonConverter(typeof(JsonStringEnumConverterEx<TypeAndLinkStatus>))]
public enum TypeAndLinkStatus
{
    [EnumMember(Value = "CHEDA Linked")]
    ChedALinked,
    [EnumMember(Value = "CHEDA Not Linked")]
    ChedANotLinked,

    [EnumMember(Value = "CHEDP Linked")]
    ChedPLinked,
    [EnumMember(Value = "CHEDP Not Linked")]
    ChedPNotLinked,

    [EnumMember(Value = "CHEDPP Linked")]
    ChedPpLinked,
    [EnumMember(Value = "CHEDPP Not Linked")]
    ChedPpNotLinked,

    [EnumMember(Value = "CHEDD Linked")]
    ChedDLinked,
    [EnumMember(Value = "CHEDD Not Linked")]
    ChedDNotLinked,
}