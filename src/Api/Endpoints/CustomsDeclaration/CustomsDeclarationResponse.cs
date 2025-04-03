namespace Defra.TradeImportsDataApi.Api.Endpoints.CustomsDeclaration;

public record CustomsDeclarationResponse(
    Domain.CustomsDeclaration.CustomsDeclaration Data,
    DateTime Created,
    DateTime Updated
);
