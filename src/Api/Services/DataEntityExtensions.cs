using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using Defra.TradeImportsDataApi.Data.Entities;
using Defra.TradeImportsDataApi.Domain.CustomsDeclaration;
using Defra.TradeImportsDataApi.Domain.Errors;
using Defra.TradeImportsDataApi.Domain.Events;
using Defra.TradeImportsDataApi.Domain.Ipaffs;
using Json.Patch;

namespace Defra.TradeImportsDataApi.Api.Services;

public static class DataEntityExtensions
{
    private static readonly JsonSerializerOptions s_jsonOptions = new()
    {
        Converters = { new JsonStringEnumConverter() },
    };

    public static ResourceEvent<CustomsDeclarationEvent> ToResourceEvent(
        this CustomsDeclarationEntity entity,
        string operation,
        CustomsDeclaration current,
        CustomsDeclaration previous,
        bool includeEntityAsResource = true
    )
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

        var entityEvent = new CustomsDeclarationEvent()
        {
            Id = entity.Id,
            ClearanceDecision = entity.ClearanceDecision,
            ClearanceRequest = entity.ClearanceRequest,
            Created = entity.Created,
            ETag = entity.ETag,
            ExternalErrors = entity.ExternalErrors,
            Finalisation = entity.Finalisation,
            Updated = entity.Updated,
        };

        return new ResourceEvent<CustomsDeclarationEvent>
        {
            ResourceId = entity.Id,
            ResourceType = ResourceTypeName<CustomsDeclarationEntity>(),
            Operation = operation,
            ETag = entity.ETag,
            Resource = includeEntityAsResource ? entityEvent : null,
            ChangeSet = operation is ResourceEventOperations.Updated ? changeSet : [],
            SubResourceType = knownSubResourceTypes.FirstOrDefault(),
        };
    }

    public static ResourceEvent<ProcessingErrorEvent> ToResourceEvent(
        this ProcessingErrorEntity entity,
        string operation,
        ProcessingError[] current,
        ProcessingError[] previous,
        bool includeEntityAsResource = true
    )
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
                    "ExternalErrors" => ResourceEventSubResourceTypes.ExternalError,
                    _ => null,
                }
            )
            .OfType<string>()
            .ToList();

        var entityEvent = new ProcessingErrorEvent()
        {
            Id = entity.Id,
            Created = entity.Created,
            ETag = entity.ETag,
            Updated = entity.Updated,
            ProcessingErrors = entity.ProcessingErrors,
        };

        return new ResourceEvent<ProcessingErrorEvent>
        {
            ResourceId = entity.Id,
            ResourceType = ResourceTypeName<ProcessingErrorEntity>(),
            Operation = operation,
            ETag = entity.ETag,
            Resource = includeEntityAsResource ? entityEvent : null,
            ChangeSet = operation is ResourceEventOperations.Updated ? changeSet : [],
            SubResourceType = knownSubResourceTypes.FirstOrDefault(),
        };
    }

    public static ResourceEvent<ImportPreNotificationEvent> ToResourceEvent(
        this ImportPreNotificationEntity entity,
        string operation,
        ImportPreNotification current,
        ImportPreNotification previous,
        bool includeEntityAsResource = true
    )
    {
        if (operation is not ResourceEventOperations.Updated and not ResourceEventOperations.Created)
            throw new ArgumentException("Operation must be either Updated or Created", nameof(operation));

        var changeSet = CreateChangeSet(current, previous);

        var entityEvent = new ImportPreNotificationEvent()
        {
            Id = entity.Id,
            ImportPreNotification = entity.ImportPreNotification,
            Created = entity.Created,
            ETag = entity.ETag,
            Updated = entity.Updated,
        };

        return new ResourceEvent<ImportPreNotificationEvent>
        {
            ResourceId = entity.Id,
            ResourceType = ResourceTypeName<ImportPreNotificationEntity>(),
            Operation = operation,
            ETag = entity.ETag,
            Resource = includeEntityAsResource ? entityEvent : null,
            ChangeSet = operation is ResourceEventOperations.Updated ? changeSet : [],
            SubResourceType = null,
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
