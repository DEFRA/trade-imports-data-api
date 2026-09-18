using System.ComponentModel.DataAnnotations;
using Defra.TradeImportsDataApi.Domain.Events;

namespace Defra.TradeImportsDataApi.Api.Configuration;

public class ResourceEventOptions
{
    [Required]
    public required string ArnPrefix { get; init; }

    [Required]
    public required string TopicName { get; init; }

    [Required]
    public required string TracesChedTopicName { get; init; }

    [Required]
    public required string ChedReservationTopicName { get; init; }

    public string TopicArn => $"{ArnPrefix}:{TopicName}";

    public string TracesChedTopicArn => $"{ArnPrefix}:{TracesChedTopicName}";

    public string ChedReservationTopicArn => $"{ArnPrefix}:{ChedReservationTopicName}";

    [Range(1, 180)]
    public int TtlDays { get; init; } = 30;

    public string GetTopicArn(string resourceType)
    {
        return resourceType switch
        {
            ResourceEventResourceTypes.TracesChed => TracesChedTopicArn,
            ResourceEventResourceTypes.ChedReservation => ChedReservationTopicArn,
            _ => TopicArn,
        };
    }
}
