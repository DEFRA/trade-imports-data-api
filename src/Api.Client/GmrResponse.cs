namespace Defra.TradeImportsDataApi.Api.Client;

public record GmrResponse(Domain.Gvms.Gmr Gmr, DateTime Created, DateTime Updated, string? ETag = null);
