using System.Text.Json;
using Defra.TradeImportsDataApi.Domain.Ipaffs;
using Defra.TradeImportsDataApi.Testing;

namespace Defra.TradeImportsDataApi.Api.IntegrationTests;

public static class ImportPreNotificationFixtures
{
    public static ImportPreNotification CreateFromSample(Type anchor, string filename)
    {
        var body = EmbeddedResource.GetBody(anchor, filename);
        return JsonSerializer.Deserialize<ImportPreNotification>(body, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true})!;
    }
}