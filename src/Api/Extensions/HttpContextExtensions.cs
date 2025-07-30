namespace Defra.TradeImportsDataApi.Api.Extensions;

public static class HttpContextExtensions
{
    public static void SetResponseEtag(this HttpContext context, string etag)
    {
        context.Response.Headers.ETag = $"\"{etag}\"";
    }

    public static void SetRequestId(this HttpContext context, string? requestId)
    {
        context.Response.Headers.Append("X-Request-ID", requestId);
    }
}
