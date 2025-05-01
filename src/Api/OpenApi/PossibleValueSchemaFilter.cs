using Defra.TradeImportsDataApi.Domain.Attributes;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Defra.TradeImportsDataApi.Api.OpenApi;

// ReSharper disable once ClassNeverInstantiated.Global
public class PossibleValueSchemaFilter : ISchemaFilter
{
    private static readonly Dictionary<string, string> s_systemVersionMap = new() { { "IPAFFS", "15.9" } };

    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        var possibleValues =
            context.MemberInfo?.CustomAttributes.Where(x => x.AttributeType == typeof(PossibleValueAttribute)).ToList()
            ?? [];

        if (possibleValues.Count != 0)
        {
            var system = s_systemVersionMap.Keys.FirstOrDefault(x =>
                context.MemberInfo?.DeclaringType?.FullName?.Contains(x, StringComparison.OrdinalIgnoreCase) ?? false
            );

            if (string.IsNullOrEmpty(system))
                throw new InvalidOperationException("Unable to determine system version.");

            if (!string.IsNullOrEmpty(schema.Description) && !schema.Description.EndsWith('.'))
                schema.Description += ".";

            schema.Description += $" Possible values taken from {system} schema version {s_systemVersionMap[system]}.";

            foreach (
                var description in possibleValues
                    .Select(possibleValue => possibleValue.ConstructorArguments.FirstOrDefault().Value?.ToString())
                    .Where(description => !string.IsNullOrWhiteSpace(description))
            )
            {
                schema.Enum.Add(new OpenApiString(description));
            }
        }
    }
}
