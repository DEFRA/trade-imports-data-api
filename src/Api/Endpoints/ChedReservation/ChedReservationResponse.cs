using System.Text.Json.Serialization;
using Defra.TradeImportsDataApi.Domain.Traces;

namespace Defra.TradeImportsDataApi.Api.Endpoints.ChedReservation;

public record ChedReservationResponse(
    [property: JsonPropertyName("reservation")] Reservation Reservation,
    [property: JsonPropertyName("created")] DateTime Created,
    [property: JsonPropertyName("updated")] DateTime Updated
);
