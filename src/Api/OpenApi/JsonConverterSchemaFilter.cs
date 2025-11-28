using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Defra.TradeImportsDataApi.Api.OpenApi;

public class JsonConverterSchemaFilter : ISchemaFilter
{
    public void Apply(IOpenApiSchema schema, SchemaFilterContext context)
    {
        var properties = schema.Properties ?? new Dictionary<string, IOpenApiSchema>();
        foreach (var property in properties)
        {
            if (
                property.Value is OpenApiSchema openApiSchema
                && context.Type == typeof(Domain.CustomsDeclaration.ImportDocument)
                && property.Key == "documentReference"
            )
            {
                openApiSchema.Type = JsonSchemaType.String;
                openApiSchema.AllOf?.Clear();
            }
        }
    }
}
