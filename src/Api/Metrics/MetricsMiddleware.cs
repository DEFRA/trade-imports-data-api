using System.Diagnostics.CodeAnalysis;

namespace Defra.TradeImportsDataApi.Api.Metrics;

[ExcludeFromCodeCoverage]
public class MetricsMiddleware(RequestMetrics requestMetrics) : IMiddleware
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
                requestMetrics.RequestCompleted(
                    path,
                    context.Request.Method,
                    context.Response.StatusCode,
                    TimeProvider.System.GetElapsedTime(startingTimestamp).TotalMilliseconds
                );
            }
        }
    }

    private static bool IgnoreRequest(string path) => s_startsWith.Any(path.StartsWith);
}
