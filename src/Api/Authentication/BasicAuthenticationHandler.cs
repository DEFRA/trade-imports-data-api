using System.Net.Http.Headers;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Encodings.Web;
using Defra.TradeImportsDataApi.Api.Configuration;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;

namespace Defra.TradeImportsDataApi.Api.Authentication;

public class BasicAuthenticationHandler(
    IOptionsMonitor<AuthenticationSchemeOptions> options,
    ILoggerFactory logger,
    UrlEncoder encoder,
    IOptions<AclOptions> aclOptions
) : AuthenticationHandler<AuthenticationSchemeOptions>(options, logger, encoder)
{
    public const string SchemeName = "Basic";

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        // Fast-path: allow anonymous
        var endpoint = Context.GetEndpoint();
        if (endpoint?.Metadata.GetMetadata<IAllowAnonymous>() != null)
            return NoResult();

        // Avoid ToString() allocation if header is missing
        if (!Request.Headers.TryGetValue(HeaderNames.Authorization, out var authHeader))
            return Fail();

        // Parse header once
        if (
            !AuthenticationHeaderValue.TryParse(authHeader, out var header)
            || !SchemeName.Equals(header.Scheme, StringComparison.OrdinalIgnoreCase)
        )
        {
            return Fail();
        }

        // Decode Base64 without intermediate strings
        var parameter = header.Parameter.AsSpan();
        Span<byte> credentialBytes = stackalloc byte[parameter.Length];
        if (!Convert.TryFromBase64Chars(parameter, credentialBytes, out int bytesWritten))
            return Fail();

        ReadOnlySpan<byte> decoded = credentialBytes[..bytesWritten];

        // Find ':' separator
        var separatorIndex = decoded.IndexOf((byte)':');
        if (separatorIndex <= 0 || separatorIndex == decoded.Length - 1)
            return Fail();

        // Decode UTF-8 slices separately (no Split, no allocations beyond strings)
        var clientId = Encoding.UTF8.GetString(decoded[..separatorIndex]);
        var secret = Encoding.UTF8.GetString(decoded[(separatorIndex + 1)..]);

        // Lookup client
        if (
            !aclOptions.Value.Clients.TryGetValue(clientId, out var client)
            || !CryptographicOperations.FixedTimeEquals(
                Encoding.UTF8.GetBytes(client.Secret),
                Encoding.UTF8.GetBytes(secret)
            )
        )
        {
            return Fail();
        }

        // Pre-size claim list
        var claims = new List<Claim>(1 + client.Scopes.Length) { new(ClaimTypes.Name, clientId) };

        foreach (var scope in client.Scopes)
            claims.Add(new Claim(Claims.Scope, scope));

        var identity = new ClaimsIdentity(claims, Scheme.Name);
        var principal = new ClaimsPrincipal(identity);

        return Success(new AuthenticationTicket(principal, Scheme.Name));
    }

    private static Task<AuthenticateResult> NoResult() => Task.FromResult(AuthenticateResult.NoResult());

    private static Task<AuthenticateResult> Fail() => Task.FromResult(AuthenticateResult.Fail("Failed authorization"));

    private static Task<AuthenticateResult> Success(AuthenticationTicket ticket) =>
        Task.FromResult(AuthenticateResult.Success(ticket));
}
