using Microsoft.Net.Http.Headers;

namespace Defra.TradeImportsData.Api.Extensions
{
    public static class HttpContextExtensions
    {
        public static void SetResponseEtag(this HttpContext context, string etag)
        {
            context.Response.Headers.Append(HeaderNames.ETag, etag);
        }
    }
}
