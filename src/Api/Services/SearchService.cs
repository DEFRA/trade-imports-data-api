using System.Text.Json;
using System.Text.Json.Serialization;
using Defra.TradeImportsDataApi.Api.Endpoints.Search;
using Defra.TradeImportsDataApi.Data.Entities;

namespace Defra.TradeImportsDataApi.Api.Services;

public class SearchService : ISearchService
{
    public Task<(
        CustomsDeclarationEntity[] customsDeclaration,
        ImportPreNotificationEntity[] importPreNotifications
    )> Search(SearchRequest searchRequest, CancellationToken cancellationToken)
    {
        return Task.FromResult(
            new ValueTuple<CustomsDeclarationEntity[], ImportPreNotificationEntity[]>(
                GetData<CustomsDeclarationEntity[]>(
                    "Defra.TradeImportsDataApi.Api.TempMockData.CustomDeclarations.json"
                ),
                GetData<ImportPreNotificationEntity[]>(
                    "Defra.TradeImportsDataApi.Api.TempMockData.PreNotifications.json"
                )
            )
        );
    }

    public static T GetData<T>(string fileName)
    {
        using var stream = typeof(SearchService).Assembly.GetManifestResourceStream(fileName);

        if (stream is null)
            throw new InvalidOperationException($"Unable to find embedded resource {fileName}");

        using var reader = new StreamReader(stream);

        return JsonSerializer.Deserialize<T>(
            reader.ReadToEnd(),
            new JsonSerializerOptions { Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) } }
        )!;
    }
}
