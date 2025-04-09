using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

namespace Defra.TradeImportsDataApi.Api.Health;

public static class WebApplicationExtensions
{
    private static readonly JsonSerializerOptions s_options = new();

    public const string Ready = "ready";
    public const string Extended = "extended";

    public static void MapHealth(this WebApplication app)
    {
        app.MapHealthChecks(
                "/health",
                new HealthCheckOptions { Predicate = healthCheck => healthCheck.Tags.Contains(Ready) }
            )
            .AllowAnonymous();

        app.MapHealthChecks(
                "/health/all",
                new HealthCheckOptions
                {
                    Predicate = _ => true,
                    ResponseWriter = (context, report) =>
                    {
                        context.Response.ContentType = "application/json; charset=utf-8";

                        var options = new JsonWriterOptions { Indented = true };

                        using var memoryStream = new MemoryStream();
                        using (var jsonWriter = new Utf8JsonWriter(memoryStream, options))
                        {
                            jsonWriter.WriteStartObject();
                            jsonWriter.WriteString("status", report.Status.ToString());
                            jsonWriter.WriteStartObject("results");

                            foreach (var entry in report.Entries)
                            {
                                jsonWriter.WriteStartObject(entry.Key);
                                jsonWriter.WriteString("status", entry.Value.Status.ToString());
                                jsonWriter.WriteString("description", entry.Value.Description);

                                if (entry.Value.Exception != null)
                                {
                                    jsonWriter.WriteString("exception", entry.Value.Exception.Message);
                                }

                                if (entry.Value.Data.Any())
                                {
                                    jsonWriter.WriteStartObject("data");

                                    foreach (var item in entry.Value.Data)
                                    {
                                        jsonWriter.WritePropertyName(JsonNamingPolicy.CamelCase.ConvertName(item.Key));
                                        JsonSerializer.Serialize(jsonWriter, item.Value, s_options);
                                    }

                                    jsonWriter.WriteEndObject();
                                }

                                jsonWriter.WriteEndObject();
                            }

                            jsonWriter.WriteEndObject();
                            jsonWriter.WriteEndObject();
                        }

                        return context.Response.WriteAsync(Encoding.UTF8.GetString(memoryStream.ToArray()));
                    },
                }
            )
            .AllowAnonymous();
    }
}
