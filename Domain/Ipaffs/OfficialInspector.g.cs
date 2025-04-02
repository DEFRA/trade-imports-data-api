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
/// Official inspector details
/// </summary>
public partial class OfficialInspector  //
{


    /// <summary>
    /// First name of inspector
    /// </summary>
    
    [JsonPropertyName("firstName")]
    [System.ComponentModel.Description("First name of inspector")]
    public string? FirstName { get; set; }


    /// <summary>
    /// Last name of inspector
    /// </summary>
    
    [JsonPropertyName("lastName")]
    [System.ComponentModel.Description("Last name of inspector")]
    public string? LastName { get; set; }


    /// <summary>
    /// Email of inspector
    /// </summary>
    
    [JsonPropertyName("email")]
    [System.ComponentModel.Description("Email of inspector")]
    public string? Email { get; set; }


    /// <summary>
    /// Phone number of inspector
    /// </summary>
    
    [JsonPropertyName("phone")]
    [System.ComponentModel.Description("Phone number of inspector")]
    public string? Phone { get; set; }


    /// <summary>
    /// Fax number of inspector
    /// </summary>
    
    [JsonPropertyName("fax")]
    [System.ComponentModel.Description("Fax number of inspector")]
    public string? Fax { get; set; }


    /// <summary>
    /// Address of inspector
    /// </summary>
    
    [JsonPropertyName("address")]
    [System.ComponentModel.Description("Address of inspector")]
    public Address? Address { get; set; }


    /// <summary>
    /// Date of sign
    /// </summary>
    
    [JsonPropertyName("signed")]
    [System.ComponentModel.Description("Date of sign")]
    public string? Signed { get; set; }

}