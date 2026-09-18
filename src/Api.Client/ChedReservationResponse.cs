using System.Text.Json.Serialization;
using Defra.TradeImportsDataApi.Domain.Traces;

namespace Defra.TradeImportsDataApi.Api.Client;

public record ChedReservationResponse(
    [property: JsonPropertyName("reservation")] Reservation Reservation,
    [property: JsonPropertyName("created")] DateTime Created,
    [property: JsonPropertyName("updated")] DateTime Updated,
    string? ETag = null
);
