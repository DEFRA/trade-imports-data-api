using Defra.TradeImportsDataApi.Domain.Attributes;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Defra.TradeImportsDataApi.Api.OpenApi;

public class PossibleValueSchemaFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        var possibleValues =
            context.MemberInfo?.CustomAttributes.Where(x => x.AttributeType == typeof(PossibleValueAttribute)).ToList()
            ?? [];

        if (possibleValues.Count != 0)
        {
            if (!string.IsNullOrEmpty(schema.Description) && !schema.Description.EndsWith('.'))
                schema.Description += ".";

            schema.Description += " Possible values taken from IPAFFS schema version 15.9.";

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
