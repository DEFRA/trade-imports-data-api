using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using Defra.TradeImportsDataApi.Data.Entities;
using Defra.TradeImportsDataApi.Domain.Events;
using Json.Patch;

namespace Defra.TradeImportsDataApi.Api.Services;

public static class ResourceEventExtensions
{
    private static readonly JsonSerializerOptions s_jsonOptions = new()
    {
        Converters = { new JsonStringEnumConverter() },
    };

    public static ResourceEvent<TDataEntity> ToResourceEvent<TDataEntity>(this TDataEntity entity, string operation)
        where TDataEntity : IDataEntity
    {
        return new ResourceEvent<TDataEntity>
        {
            ResourceId = entity.Id,
            ResourceType = ResourceTypeName<TDataEntity>(),
            Operation = operation,
            ETag = entity.ETag,
            Resource = entity,
        };
    }

    private static string ResourceTypeName<TDataEntity>()
        where TDataEntity : IDataEntity
    {
        var name = typeof(TDataEntity).Name.Replace("Entity", string.Empty);

        return name switch
        {
            ResourceEventResourceTypes.ImportPreNotification => ResourceEventResourceTypes.ImportPreNotification,
            ResourceEventResourceTypes.CustomsDeclaration => ResourceEventResourceTypes.CustomsDeclaration,
            _ => name,
        };
    }

    public static ResourceEvent<TDataEntity> WithChangeSet<TDataEntity, TDomain>(
        this ResourceEvent<TDataEntity> @event,
        TDomain current,
        TDomain previous
    )
        where TDataEntity : IDataEntity
        where TDomain : class
    {
        var changeSet = CreateChangeSet(current, previous);
        var knownChildResourceTypes = changeSet
            .Select(x => x.Path[1..])
            .Distinct()
            .Select(x =>
                x switch
                {
                    ResourceEventChildResourceTypes.ClearanceRequest =>
                        ResourceEventChildResourceTypes.ClearanceRequest,
                    ResourceEventChildResourceTypes.ClearanceDecision =>
                        ResourceEventChildResourceTypes.ClearanceDecision,
                    ResourceEventChildResourceTypes.Finalisation => ResourceEventChildResourceTypes.Finalisation,
                    _ => null,
                }
            )
            .OfType<string>()
            .ToList();

        if (knownChildResourceTypes.Count > 1)
            throw new InvalidOperationException("Change set contains multiple child resource types");

        return @event with
        {
            ChangeSet = changeSet,
            ChildResourceType = knownChildResourceTypes.FirstOrDefault(),
        };
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
            .Operations.Select(x => new Diff
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
