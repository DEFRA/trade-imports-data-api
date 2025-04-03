using Defra.TradeImportsData.Domain.Gvms;

namespace Defra.TradeImportsData.Api.Endpoints.Gmrs;

public record GmrResponse(Gmr Data, DateTime Created, DateTime Updated);
