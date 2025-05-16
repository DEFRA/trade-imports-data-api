using System.Text.Json;
using Defra.TradeImportsDataApi.Domain.Ipaffs;
using Defra.TradeImportsDataApi.Testing;

namespace Defra.TradeImportsDataApi.Api.IntegrationTests;

public static class ImportPreNotificationFixtures
{
    private static readonly JsonSerializerOptions s_options = new() { PropertyNameCaseInsensitive = true };

    public static ImportPreNotification CreateFromSample(Type anchor, string filename)
    {
        var body = EmbeddedResource.GetBody(anchor, filename);

        return JsonSerializer.Deserialize<ImportPreNotification>(body, s_options)!;
    }
}
