using System.Net;
using System.Net.Http.Json;
using Defra.TradeImportsDataApi.Api.Exceptions;
using Defra.TradeImportsDataApi.Api.Services;
using Defra.TradeImportsDataApi.Data;
using Defra.TradeImportsDataApi.Data.Entities;
using Defra.TradeImportsDataApi.Domain.Errors;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Xunit.Abstractions;

namespace Defra.TradeImportsDataApi.Api.Tests.Endpoints.ProcessingErrors;

public class PutTests(ApiWebApplicationFactory factory, ITestOutputHelper outputHelper)
    : EndpointTestBase(factory, outputHelper)
{
    private const string Mrn = "mrn";
    private IProcessingErrorService MockProcessingErrorService { get; } = Substitute.For<IProcessingErrorService>();

    protected override void ConfigureTestServices(IServiceCollection services)
    {
        base.ConfigureTestServices(services);

        services.AddTransient<IProcessingErrorService>(_ => MockProcessingErrorService);
    }

    [Fact]
    public async Task Put_WhenUnauthorized_ShouldBeUnauthorized()
    {
        var client = CreateClient(addDefaultAuthorizationHeader: false);

        var response = await client.PutAsJsonAsync(Testing.Endpoints.ProcessingErrors.Put(Mrn), new ProcessingError());

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Put_WhenReadOnly_ShouldBeForbidden()
    {
        var client = CreateClient(testUser: TestUser.ReadOnly);

        var response = await client.PutAsJsonAsync(Testing.Endpoints.ProcessingErrors.Put(Mrn), new ProcessingError());

        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task Put_WhenEntityNotFound_ShouldBeNotFound()
    {
        var client = CreateClient();
        MockProcessingErrorService
            .Insert(Arg.Any<ProcessingErrorEntity>(), Arg.Any<CancellationToken>())
            .Throws(new EntityNotFoundException("entityType", "entityId"));

        var response = await client.PutAsJsonAsync(Testing.Endpoints.ProcessingErrors.Put(Mrn), new ProcessingError());

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Put_WhenConcurrencyException_ShouldBeConflict()
    {
        var client = CreateClient();
        MockProcessingErrorService
            .Insert(Arg.Any<ProcessingErrorEntity>(), Arg.Any<CancellationToken>())
            .Throws(new ConcurrencyException("entityId", "etag"));

        var response = await client.PutAsJsonAsync(Testing.Endpoints.ProcessingErrors.Put(Mrn), new ProcessingError());

        response.StatusCode.Should().Be(HttpStatusCode.Conflict);
    }
}
