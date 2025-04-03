namespace Defra.TradeImportsDataApi.Api.Endpoints.CustomsDeclarations;

public record CustomsDeclarationResponse(
    Domain.CustomsDeclaration.CustomsDeclaration Data,
    DateTime Created,
    DateTime Updated
);
