using System.Reflection;
using Defra.TradeImportsDataApi.Domain.Ipaffs;
using Microsoft.OpenApi.Models;

namespace Defra.TradeImportsDataApi.Api.OpenApi;

public static class ServiceCollectionExtensions
{
    public static void AddOpenApi(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSwaggerGen(c =>
        {
            c.AddServer(
                new OpenApiServer { Url = "https://" + (configuration.GetValue<string>("OpenApi:Host") ?? "localhost") }
            );
            c.AddSecurityDefinition(
                "Basic",
                new OpenApiSecurityScheme
                {
                    Description = "RFC8725 Compliant JWT",
                    In = ParameterLocation.Header,
                    Name = "Authorization",
                    Scheme = "Basic",
                    Type = SecuritySchemeType.Http,
                }
            );
            c.AddSecurityRequirement(
                new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Basic" },
                        },
                        []
                    },
                }
            );
            c.IncludeXmlComments(Assembly.GetExecutingAssembly());
            c.IncludeXmlComments(typeof(ImportPreNotification).Assembly);
            c.SchemaFilter<PossibleValueSchemaFilter>();
            c.SchemaFilter<JsonConverterSchemaFilter>();
            c.OperationFilter<PossibleValueOperationFilter>();

            var typeMap = new Dictionary<string, string>
            {
                // ReSharper disable once RedundantNameQualifier
                {
                    typeof(Defra.TradeImportsDataApi.Domain.Ipaffs.CommodityCheck).FullName!,
                    "NotificationCommodityCheck"
                },
                {
                    typeof(Defra.TradeImportsDataApi.Domain.CustomsDeclaration.CommodityCheck).FullName!,
                    "CustomsCommodityCheck"
                },
            };
            c.CustomSchemaIds(x =>
            {
                var schemaId = x.FullName!;

                if (schemaId.StartsWith("Defra"))
                {
                    var typeName = typeMap.TryGetValue(x.FullName!, out var mappedTypeName) ? mappedTypeName : x.Name;
                    schemaId = "Defra.TradeImportsDataApi." + typeName;
                }

                return schemaId;
            });

            c.SupportNonNullableReferenceTypes();
            c.UseAllOfToExtendReferenceSchemas();
            c.SwaggerDoc(
                "v1",
                new OpenApiInfo
                {
                    Description = "TBC",
                    Contact = new OpenApiContact
                    {
                        Email = "tbc@defra.gov.uk",
                        Name = "DEFRA",
                        Url = new Uri(
#pragma warning disable S1075
                            "https://www.gov.uk/government/organisations/department-for-environment-food-rural-affairs"
#pragma warning restore S1075
                        ),
                    },
                    Title = "Trade Imports Data API",
                    Version = "v1",
                }
            );
        });
    }
}
