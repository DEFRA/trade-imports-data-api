using System.ComponentModel.DataAnnotations;

namespace Defra.TradeImportsDataApi.Api.Configuration;

public class ResourceEventOptions
{
    [Required]
    public required string ArnPrefix { get; init; }

    [Required]
    public required string TopicName { get; init; }

    public string TopicArn => $"{ArnPrefix}:{TopicName}";

    [Range(1, 180)]
    public int TtlDays { get; init; } = 30;
}
