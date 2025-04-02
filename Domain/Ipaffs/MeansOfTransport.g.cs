//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
#nullable enable


using System.Text.Json.Serialization;



namespace Defra.TradeImportsData.Domain.IPaffs;

/// <summary>
/// Details of transport
/// </summary>
public partial class MeansOfTransport  //
{


    /// <summary>
    /// Type of transport
    /// </summary>
    
    [JsonPropertyName("type")]
    [System.ComponentModel.Description("Type of transport")]
    [MongoDB.Bson.Serialization.Attributes.BsonRepresentation(MongoDB.Bson.BsonType.String)]
    public MeansOfTransportTypeEnum? Type { get; set; }


    /// <summary>
    /// Document for transport
    /// </summary>
    
    [JsonPropertyName("document")]
    [System.ComponentModel.Description("Document for transport")]
    public string? Document { get; set; }


    /// <summary>
    /// ID of transport
    /// </summary>
    
    [JsonPropertyName("id")]
    [System.ComponentModel.Description("ID of transport")]
    public string? Id { get; set; }

}