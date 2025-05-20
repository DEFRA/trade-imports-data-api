using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Defra.TradeImportsDataApi.Api.OpenApi;

public class JsonConverterSchemaFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        foreach (var property in schema.Properties)
        {
            if (context.Type == typeof(Domain.CustomsDeclaration.ImportDocument) && property.Key == "documentReference")
            {
                property.Value.Type = "string";
                property.Value.AllOf.Clear();
            }
        }
    }
}
