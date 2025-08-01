using System.Text.Json.Serialization;

namespace Defra.TradeImportsDataApi.Data.Entities;

// This entity is new. Therefore, we can add the JsonPropertyName attributes
// that should be on the other entities, which will be added in due course
// as part of a future change

public class ResourceEventEntity : IDataEntity
{
    [JsonPropertyName("id")]
    public required string Id { get; set; }

    [JsonPropertyName("etag")]
    public string ETag { get; set; } = null!;

    [JsonPropertyName("created")]
    public DateTime Created { get; set; }

    [JsonPropertyName("updated")]
    public DateTime Updated { get; set; }

    [JsonPropertyName("resourceId")]
    public required string ResourceId { get; set; }

    [JsonPropertyName("resourceType")]
    public required string ResourceType { get; set; }

    [JsonPropertyName("subResourceType")]
    public string? SubResourceType { get; set; }

    [JsonPropertyName("message")]
    public required string Message { get; set; }

    [JsonPropertyName("published")]
    public DateTime? Published { get; set; }

    [JsonPropertyName("expiresAt")]
    public DateTime? ExpiresAt { get; set; }

    public void OnSave() { }
}
