using System.Net;
using System.Net.Http.Json;
using Defra.TradeImportsDataApi.Api.Exceptions;
using Defra.TradeImportsDataApi.Api.Services;
using Defra.TradeImportsDataApi.Data;
using Defra.TradeImportsDataApi.Data.Entities;
using Defra.TradeImportsDataApi.Domain.CustomsDeclaration;
using Defra.TradeImportsDataApi.Domain.Ipaffs;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Xunit.Abstractions;

namespace Defra.TradeImportsDataApi.Api.Tests.Endpoints.CustomsDeclarations;

public class PutTests(ApiWebApplicationFactory factory, ITestOutputHelper outputHelper)
    : EndpointTestBase(factory, outputHelper)
{
    private const string Mrn = "mrn";
    private ICustomsDeclarationService MockCustomsDeclarationService { get; } =
        Substitute.For<ICustomsDeclarationService>();

    protected override void ConfigureTestServices(IServiceCollection services)
    {
        base.ConfigureTestServices(services);

        services.AddTransient<ICustomsDeclarationService>(_ => MockCustomsDeclarationService);
    }

    [Fact]
    public async Task Put_WhenUnauthorized_ShouldBeUnauthorized()
    {
        var client = CreateClient(addDefaultAuthorizationHeader: false);

        var response = await client.PutAsJsonAsync(
            Testing.Endpoints.CustomsDeclarations.Put(Mrn),
            new CustomsDeclaration()
        );

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Put_WhenReadOnly_ShouldBeForbidden()
    {
        var client = CreateClient(testUser: TestUser.ReadOnly);

        var response = await client.PutAsJsonAsync(
            Testing.Endpoints.CustomsDeclarations.Put(Mrn),
            new CustomsDeclaration()
        );

        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task Put_WhenEntityNotFound_ShouldBeNotFound()
    {
        var client = CreateClient();
        MockCustomsDeclarationService
            .Insert(Arg.Any<CustomsDeclarationEntity>(), Arg.Any<CancellationToken>())
            .Throws(new EntityNotFoundException("entityType", "entityId"));

        var response = await client.PutAsJsonAsync(
            Testing.Endpoints.CustomsDeclarations.Put(Mrn),
            new CustomsDeclaration()
        );

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Put_WhenConcurrencyException_ShouldBeConflict()
    {
        var client = CreateClient();
        MockCustomsDeclarationService
            .Insert(Arg.Any<CustomsDeclarationEntity>(), Arg.Any<CancellationToken>())
            .Throws(new ConcurrencyException("entityId", "etag"));

        var response = await client.PutAsJsonAsync(
            Testing.Endpoints.CustomsDeclarations.Put(Mrn),
            new CustomsDeclaration()
        );

        response.StatusCode.Should().Be(HttpStatusCode.Conflict);
    }
}
