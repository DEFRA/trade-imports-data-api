using Defra.TradeImportsDataApi.Domain.Gvms;

namespace Defra.TradeImportsDataApi.Api.Endpoints.Gmrs;

public record GmrResponse(Gmr Gmr, DateTime Created, DateTime Updated);
