using Defra.TradeImportsDataApi.Domain.CustomsDeclaration;

namespace Defra.TradeImportsDataApi.Api.Endpoints.CustomsDeclarations;

public record CustomsDeclarationResponse(
    string MovementReferenceNumber,
    ClearanceRequest? ClearanceRequest,
    ClearanceDecision? ClearanceDecision,
    Finalisation? Finalisation,
    DateTime Created,
    DateTime Updated
);
