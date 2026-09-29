using Defra.TradeImportsDataApi.Data.Entities;

namespace Defra.TradeImportsDataApi.Data.Extensions;

public static class CustomsDeclarationIdentifierQueryableExtensions
{
    private static readonly string[] s_versionSuffixes = ["V", "R"];

    /// <summary>
    /// Returns the ID of the entity with the highest customs declaration identifier. Any trailing
    /// version suffix is ignored, and longer identifiers are considered higher than shorter ones
    /// so ordering is numeric rather than lexical once the sequence rolls over to more digits.
    /// </summary>
    public static Task<string?> GetMaxIdByCustomsDeclarationIdentifier<T>(
        this IQueryable<T> source,
        CancellationToken cancellationToken
    )
        where T : ICustomsDeclarationIdentifierEntity =>
        source
            .Where(x => x.CustomsDeclarationIdentifier != null && x.CustomsDeclarationIdentifier != "")
            .Select(x => new
            {
                x.Id,
                Identifier = s_versionSuffixes.Contains(
                    x.CustomsDeclarationIdentifier.Substring(x.CustomsDeclarationIdentifier.Length - 1, 1)
                )
                    ? x.CustomsDeclarationIdentifier.Substring(0, x.CustomsDeclarationIdentifier.Length - 1)
                    : x.CustomsDeclarationIdentifier,
            })
            .OrderByDescending(x => x.Identifier.Length)
            .ThenByDescending(x => x.Identifier)
            .Select(x => (string?)x.Id)
            .FirstOrDefaultWithFallbackAsync(cancellationToken);
}
