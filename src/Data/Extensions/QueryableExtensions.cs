using MongoDB.Driver;

namespace Defra.TradeImportsDataApi.Data.Extensions;

public static class QueryableExtensions
{
    public static async Task<List<TSource>> ToListWithFallbackAsync<TSource>(
        this IQueryable<TSource> source,
        CancellationToken cancellationToken = default
    )
    {
        if (source is IAsyncCursorSource<TSource> cursorSource)
        {
            return await cursorSource.ToListAsync(cancellationToken);
        }

        return source.AsEnumerable().ToList();
    }

    public static async Task<TSource?> FirstOrDefaultWithFallbackAsync<TSource>(
        this IQueryable<TSource> source,
        CancellationToken cancellationToken = default
    )
    {
        if (source is IAsyncCursorSource<TSource> cursorSource)
        {
            return await cursorSource.FirstOrDefaultAsync(cancellationToken);
        }

        return source.AsEnumerable().FirstOrDefault();
    }
}
