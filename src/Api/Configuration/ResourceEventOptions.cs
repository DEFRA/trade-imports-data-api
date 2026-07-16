using Defra.TradeImportsDataApi.Domain.Events;
using System.ComponentModel.DataAnnotations;

namespace Defra.TradeImportsDataApi.Api.Configuration;

public class ResourceEventOptions
{
    [Required]
    public required string ArnPrefix { get; init; }

    [Required]
    public required string TopicName { get; init; }

    [Required]
    public required string TracesChedTopicName { get; init; }

    public string TopicArn => $"{ArnPrefix}:{TopicName}";

    public string TracesChedTopicArn => $"{ArnPrefix}:{TracesChedTopicName}";

    [Range(1, 180)]
    public int TtlDays { get; init; } = 30;

    public string GetTopicArn(string resourceType)
    {
        return ResourceEventResourceTypes.TracesChed == resourceType ? TracesChedTopicArn : TopicArn;
    }
}
