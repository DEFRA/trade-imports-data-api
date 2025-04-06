using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using Defra.TradeImportsDataApi.Api.Configuration;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

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
        var authorizationHeader = Request.Headers.Authorization.ToString();
        if (string.IsNullOrEmpty(authorizationHeader))
            return Fail();

        var authenticationHeaderValue = AuthenticationHeaderValue.Parse(Request.Headers.Authorization.ToString());
        if (authenticationHeaderValue.Scheme != SchemeName)
            return Fail();

        var credentialBytes = Convert.FromBase64String(authenticationHeaderValue.Parameter ?? string.Empty);
        var credentials = Encoding.UTF8.GetString(credentialBytes).Split(':', 2);
        var clientId = credentials[0];

        aclOptions.Value.Clients.TryGetValue(clientId, out var client);
        if (client == null)
            return Fail();

        if (client.Secret != credentials[1])
            return Fail();

        var claims = new List<Claim> { new(ClaimTypes.Name, clientId) };
        claims.AddRange(client.Scopes.Select(scope => new Claim(Claims.Scope, scope)));

        var identity = new ClaimsIdentity(claims, Scheme.Name);
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, Scheme.Name);

        return Success(ticket);
    }

    private static Task<AuthenticateResult> Fail() => Task.FromResult(AuthenticateResult.Fail("Failed authorization"));

    private static Task<AuthenticateResult> Success(AuthenticationTicket ticket) =>
        Task.FromResult(AuthenticateResult.Success(ticket));
}
