using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using Defra.TradeImportsDataApi.Data.Entities;
using Defra.TradeImportsDataApi.Domain.Events;
using Json.Patch;

namespace Defra.TradeImportsDataApi.Api.Services.Extensions;

public static class ResourceEventExtensions
{
    // Do we need this specific serializer options?
    private static readonly JsonSerializerOptions s_jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        NumberHandling = JsonNumberHandling.AllowReadingFromString,
        Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) },
    };

    public static ResourceEvent<TDataEntity> ToResourceEvent<TDataEntity>(this TDataEntity entity, string operation)
        where TDataEntity : IDataEntity
    {
        return new ResourceEvent<TDataEntity>
        {
            ResourceId = entity.Id,
            ResourceType = typeof(TDataEntity).Name,
            Operation = operation,
        };
    }

    public static ResourceEvent<TDataEntity> WithChangeSet<TDataEntity, TDomain>(
        this ResourceEvent<TDataEntity> @event,
        TDomain from,
        TDomain to
    )
        where TDataEntity : IDataEntity
        where TDomain : class
    {
        return @event with { ChangeSet = CreateChangeSet(from, to) };
    }

    private static List<Diff> CreateChangeSet<T>([DisallowNull] T current, [DisallowNull] T previous)
        where T : class
    {
        ArgumentNullException.ThrowIfNull(current);
        ArgumentNullException.ThrowIfNull(previous);

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

        return JsonSerializer.Serialize(value, s_jsonOptions);
    }
}
