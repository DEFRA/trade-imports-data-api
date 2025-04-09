using System.Text;
using Defra.TradeImportsDataApi.Api.Authentication;
using Defra.TradeImportsDataApi.Api.Configuration;
using FluentAssertions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.WebEncoders.Testing;
using NSubstitute;

namespace Defra.TradeImportsDataApi.Api.Tests.Authentication;

public class BasicAuthenticationHandlerTests
{
    private BasicAuthenticationHandler Subject { get; }
    private IOptionsMonitor<AuthenticationSchemeOptions> OptionsMonitor { get; } =
        Substitute.For<IOptionsMonitor<AuthenticationSchemeOptions>>();
    private AclOptions AclOptions { get; set; } = new();

    public BasicAuthenticationHandlerTests()
    {
        Subject = new BasicAuthenticationHandler(
            OptionsMonitor,
            Substitute.For<ILoggerFactory>(),
            new UrlTestEncoder(),
            new OptionsWrapper<AclOptions>(AclOptions)
        );

        OptionsMonitor.Get("Basic").Returns(new AuthenticationSchemeOptions());
    }

    [Fact]
    public async Task WhenNoAuthorizationHeader_ShouldFail()
    {
        await Subject.InitializeAsync(Scheme(), new DefaultHttpContext());

        await AuthenticateAndAssertFailure();
    }

    [Fact]
    public async Task WhenInvalidAuthorizationHeaderScheme_ShouldFail()
    {
        await Subject.InitializeAsync(
            Scheme(),
            new DefaultHttpContext { Request = { Headers = { Authorization = "InvalidScheme Value" } } }
        );

        await AuthenticateAndAssertFailure();
    }

    [Fact]
    public async Task WhenNoCredentials_ShouldFail()
    {
        await Subject.InitializeAsync(
            Scheme(),
            new DefaultHttpContext { Request = { Headers = { Authorization = "Basic " } } }
        );

        await AuthenticateAndAssertFailure();
    }

    [Theory]
    [InlineData(":secret")]
    [InlineData("username:")]
    public async Task WhenInvalidCredentials_ShouldFail(string credentials)
    {
        await Subject.InitializeAsync(
            Scheme(),
            new DefaultHttpContext
            {
                Request =
                {
                    Headers =
                    {
                        Authorization = $"Basic {Convert.ToBase64String(Encoding.UTF8.GetBytes(credentials))}",
                    },
                },
            }
        );

        await AuthenticateAndAssertFailure();
    }

    [Fact]
    public async Task WhenNoMatchingClientId_ShouldFail()
    {
        AclOptions.Clients.Add("different-client", new AclOptions.Client { Secret = "secret", Scopes = [] });
        await Subject.InitializeAsync(
            Scheme(),
            new DefaultHttpContext
            {
                Request =
                {
                    Headers = { Authorization = $"Basic {Convert.ToBase64String("client:secret"u8.ToArray())}" },
                },
            }
        );

        await AuthenticateAndAssertFailure();
    }

    [Fact]
    public async Task WhenNoMatchingClientSecret_ShouldFail()
    {
        AclOptions.Clients.Add("client", new AclOptions.Client { Secret = "different-secret", Scopes = [] });
        await Subject.InitializeAsync(
            Scheme(),
            new DefaultHttpContext
            {
                Request =
                {
                    Headers = { Authorization = $"Basic {Convert.ToBase64String("client:secret"u8.ToArray())}" },
                },
            }
        );

        await AuthenticateAndAssertFailure();
    }

    private static AuthenticationScheme Scheme()
    {
        return new AuthenticationScheme("Basic", "Basic", typeof(BasicAuthenticationHandler));
    }

    private async Task AuthenticateAndAssertFailure()
    {
        var result = await Subject.AuthenticateAsync();

        result.Failure.Should().NotBeNull();
        result.Failure.Should().BeOfType<AuthenticationFailureException>();
    }
}
