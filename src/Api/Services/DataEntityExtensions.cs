using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using Defra.TradeImportsDataApi.Data.Entities;
using Defra.TradeImportsDataApi.Domain.Events;
using Json.Patch;

namespace Defra.TradeImportsDataApi.Api.Services;

public static class DataEntityExtensions
{
    private static readonly JsonSerializerOptions s_jsonOptions = new()
    {
        Converters = { new JsonStringEnumConverter() },
    };

    public static ResourceEvent<TDataEntity> ToResourceEvent<TDataEntity, TDomain>(
        this TDataEntity entity,
        string operation,
        TDomain current,
        TDomain previous,
        bool includeEntityAsResource = true
    )
        where TDataEntity : IDataEntity
        where TDomain : class
    {
        if (operation is not ResourceEventOperations.Updated and not ResourceEventOperations.Created)
            throw new ArgumentException("Operation must be either Updated or Created", nameof(operation));

        var changeSet = CreateChangeSet(current, previous);
        var knownSubResourceTypes = changeSet
            .Select(x => x.Path.Split('/', StringSplitOptions.RemoveEmptyEntries).FirstOrDefault())
            .Distinct()
            .Select(x =>
                x switch
                {
                    ResourceEventSubResourceTypes.ClearanceRequest => ResourceEventSubResourceTypes.ClearanceRequest,
                    ResourceEventSubResourceTypes.ClearanceDecision => ResourceEventSubResourceTypes.ClearanceDecision,
                    ResourceEventSubResourceTypes.Finalisation => ResourceEventSubResourceTypes.Finalisation,
                    "ExternalErrors" => ResourceEventSubResourceTypes.ExternalError,
                    _ => null,
                }
            )
            .OfType<string>()
            .ToList();

        if (knownSubResourceTypes.Count > 1)
            throw new InvalidOperationException(
                $"Change set contains multiple known sub resource types \"{string.Join(", ", knownSubResourceTypes)}\", only one changing at a time is currently expected"
            );

        return new ResourceEvent<TDataEntity>
        {
            ResourceId = entity.Id,
            ResourceType = ResourceTypeName<TDataEntity>(),
            Operation = operation,
            ETag = entity.ETag,
            Resource = includeEntityAsResource ? entity : default,
            ChangeSet = operation is ResourceEventOperations.Updated ? changeSet : [],
            SubResourceType = knownSubResourceTypes.FirstOrDefault(),
        };
    }

    private static string ResourceTypeName<TDataEntity>()
        where TDataEntity : IDataEntity
    {
        var name = typeof(TDataEntity).DataEntityName();

        return name switch
        {
            ResourceEventResourceTypes.ImportPreNotification => ResourceEventResourceTypes.ImportPreNotification,
            ResourceEventResourceTypes.CustomsDeclaration => ResourceEventResourceTypes.CustomsDeclaration,
            ResourceEventResourceTypes.ProcessingError => ResourceEventResourceTypes.ProcessingError,
            _ => name,
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
