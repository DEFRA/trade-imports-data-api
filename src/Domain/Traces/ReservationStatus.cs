using System.Diagnostics.CodeAnalysis;

namespace Defra.TradeImportsDataApi.Domain.Traces;

[ExcludeFromCodeCoverage]
public static class ReservationStatus
{
    public const string Unreserved = nameof(Unreserved);
    public const string Reserved = nameof(Reserved);
    public const string Consumed = nameof(Consumed);
    public const string Unsuccessful = nameof(Unsuccessful);
}
