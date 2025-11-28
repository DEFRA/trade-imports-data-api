using System.Reflection;
using Defra.TradeImportsDataApi.Domain.Attributes;
using Defra.TradeImportsDataApi.Domain.Ipaffs;
using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Defra.TradeImportsDataApi.Api.OpenApi;

// ReSharper disable once ClassNeverInstantiated.Global
public class PossibleValueOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        HandleNotificationUpdatesTypeQueryParam(operation);
        HandleNotificationUpdatesStatusQueryParam(operation);
    }

    private static void HandleNotificationUpdatesTypeQueryParam(OpenApiOperation operation)
    {
        var parameter = operation.Parameters?.FirstOrDefault(p =>
            operation.OperationId == "GetImportPreNotificationUpdates"
            && p.In == ParameterLocation.Query
            && p.Name == "type"
        );

        if (parameter == null)
            return;

        AppendPossibleValues(
            typeof(ImportPreNotification).GetMember(nameof(ImportPreNotification.ImportNotificationType)),
            parameter
        );
    }

    private static void HandleNotificationUpdatesStatusQueryParam(OpenApiOperation operation)
    {
        var parameter = operation.Parameters?.FirstOrDefault(p =>
            operation.OperationId == "GetImportPreNotificationUpdates"
            && p.In == ParameterLocation.Query
            && p.Name == "status"
        );

        if (parameter == null)
            return;

        AppendPossibleValues(typeof(ImportPreNotification).GetMember(nameof(ImportPreNotification.Status)), parameter);
    }

    private static void AppendPossibleValues(MemberInfo[] member, IOpenApiParameter parameter)
    {
        var possibleValues =
            member
                .FirstOrDefault()
                ?.CustomAttributes.Where(x => x.AttributeType == typeof(PossibleValueAttribute))
                .ToList() ?? [];
        var descriptions = possibleValues
            .Select(possibleValue => possibleValue.ConstructorArguments.FirstOrDefault().Value?.ToString())
            .Where(description => !string.IsNullOrWhiteSpace(description));

        parameter.Description = parameter.Description + " Possible values: " + string.Join(", ", descriptions);
    }
}
