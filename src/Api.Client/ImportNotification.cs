namespace Defra.TradeImportsDataApi.Api.Client;

public record ImportNotification(Domain.Ipaffs.ImportNotification Data, DateTime Created, DateTime Updated);

public record ClientResponse<T>(T Resource, string? ETag);
