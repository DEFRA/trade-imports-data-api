using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using Defra.TradeImportsDataApi.Domain.Events;
using Json.Patch;

namespace Defra.TradeImportsDataApi.Api.Utils;

public static class DiffExtensions
{
    private static readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        NumberHandling = JsonNumberHandling.AllowReadingFromString,
        Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) },
    };

    public static List<Diff> CreateDiff<T>([DisallowNull] T current, [DisallowNull] T previous)
        where T : class
    {
        if (current == null)
            throw new ArgumentNullException(nameof(current));
        if (previous == null)
            throw new ArgumentNullException(nameof(previous));

        var previousNode = JsonNode.Parse(ToJsonString(previous));
        var currentNode = JsonNode.Parse(ToJsonString(current));
        var diff = previousNode.CreatePatch(currentNode);

        return diff
            .Operations.Select(x => new Diff()
            {
                Path = x.Path.ToString(),
                Operation = x.Op.ToString(),
                Value = x.Value?.ToString(),
            })
            .ToList();
    }

    private static string ToJsonString<T>(T value)
    {
        if (value is string s)
            return s;

        return JsonSerializer.Serialize(value, _jsonOptions);
    }
}
