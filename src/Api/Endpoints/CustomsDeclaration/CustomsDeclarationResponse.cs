using Defra.TradeImportsData.Domain.CustomsDeclaration;

namespace Defra.TradeImportsData.Api.Endpoints.CustomsDeclaration;

public record CustomsDeclarationResponse(TradeImportsData.Domain.CustomsDeclaration.CustomsDeclaration Data, DateTime Created, DateTime Updated);
