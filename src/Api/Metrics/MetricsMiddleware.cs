using System.Diagnostics.CodeAnalysis;

namespace Defra.TradeImportsDataApi.Api.Metrics;

[ExcludeFromCodeCoverage]
public class MetricsMiddleware(RequestMetrics requestMetrics, ILogger<MetricsMiddleware> logger) : IMiddleware
{
    private static readonly string[] s_startsWith = ["/apple-", "/favicon", "/redoc", "/.well-known/openapi"];

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var startingTimestamp = TimeProvider.System.GetTimestamp();
        var path = (context.GetEndpoint() as RouteEndpoint)?.RoutePattern.RawText ?? "unknown";

        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            requestMetrics.RequestFaulted(path, context.Request.Method, context.Response.StatusCode, ex);
        }
        finally
        {
            if (!IgnoreRequest(path))
            {
                var totalMilliseconds = TimeProvider.System.GetElapsedTime(startingTimestamp).TotalMilliseconds;
                requestMetrics.RequestCompleted(
                    path,
                    context.Request.Method,
                    context.Response.StatusCode,
                    totalMilliseconds
                );

                if (totalMilliseconds > 750)
                {
                    logger.LogWarning(
                        "Slow request - {Method} : {Path} : {StatusCode} : {TotalMilliseconds}",
                        context.Request.Method,
                        context.Request.Path,
                        context.Response.StatusCode,
                        totalMilliseconds
                    );
                }
            }
        }
    }

    private static bool IgnoreRequest(string path) => s_startsWith.Any(path.StartsWith);
}
