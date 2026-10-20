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

    [Required]
    public required string IuuNotificationTopicName { get; init; }

    public string TopicArn => $"{ArnPrefix}:{TopicName}";

    public string TracesChedTopicArn => $"{ArnPrefix}:{TracesChedTopicName}";

    public string ChedReservationTopicArn => $"{ArnPrefix}:{ChedReservationTopicName}";

    public string IuuNotificationTopicArn => $"{ArnPrefix}:{IuuNotificationTopicName}";

    [Range(1, 180)]
    public int TtlDays { get; init; } = 30;

    public string GetTopicArn(string resourceType)
    {
        return resourceType switch
        {
            ResourceEventResourceTypes.TracesChed => TracesChedTopicArn,
            ResourceEventResourceTypes.ChedReservation => ChedReservationTopicArn,
            ResourceEventResourceTypes.IuuNotification => IuuNotificationTopicArn,
            _ => TopicArn,
        };
    }
}
