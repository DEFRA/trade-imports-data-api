using System.Diagnostics.CodeAnalysis;

namespace Defra.TradeImportsDataApi.Domain.Events;

[ExcludeFromCodeCoverage]
public static class ResourceEventChildResourceTypes
{
    public const string ClearanceRequest = nameof(ClearanceRequest);

    public const string ClearanceDecision = nameof(ClearanceDecision);

    public const string Finalisation = nameof(Finalisation);
}
