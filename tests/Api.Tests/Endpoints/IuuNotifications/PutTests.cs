using System.Net;
using System.Net.Http.Json;
using Defra.TradeImportsDataApi.Api.Exceptions;
using Defra.TradeImportsDataApi.Api.Services;
using Defra.TradeImportsDataApi.Data;
using Defra.TradeImportsDataApi.Data.Entities;
using Defra.TradeImportsDataApi.Domain.Iuu;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Xunit.Abstractions;

namespace Defra.TradeImportsDataApi.Api.Tests.Endpoints.IuuNotifications;

public class PutTests(ApiWebApplicationFactory factory, ITestOutputHelper outputHelper)
    : EndpointTestBase(factory, outputHelper)
{
    private const string IuuReference = "IUU.FR.2026.0000001";
    private IIuuNotificationService MockIuuNotificationService { get; } = Substitute.For<IIuuNotificationService>();

    protected override void ConfigureTestServices(IServiceCollection services)
    {
        base.ConfigureTestServices(services);

        services.AddTransient<IIuuNotificationService>(_ => MockIuuNotificationService);
    }

    private static IuuNotification CreateIuuNotification() =>
        new()
        {
            Reference = IuuReference,
            LastUpdated = new DateTime(2026, 10, 16, 9, 0, 0, DateTimeKind.Utc),
            IuuOption = "IUUOK",
        };

    [Fact]
    public async Task Put_WhenUnauthorized_ShouldBeUnauthorized()
    {
        var client = CreateClient(addDefaultAuthorizationHeader: false);

        var response = await client.PutAsJsonAsync(
            Testing.Endpoints.IuuNotifications.Put(IuuReference),
            CreateIuuNotification()
        );

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Put_WhenReadOnly_ShouldBeForbidden()
    {
        var client = CreateClient(testUser: TestUser.ReadOnly);

        var response = await client.PutAsJsonAsync(
            Testing.Endpoints.IuuNotifications.Put(IuuReference),
            CreateIuuNotification()
        );

        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task Put_WhenNoEtag_ShouldInsertAndReturnOk()
    {
        var client = CreateClient();
        var iuuNotification = CreateIuuNotification();
        var entity = new IuuNotificationEntity()
        {
            Id = IuuReference,
            IuuNotification = iuuNotification,
            Created = new DateTime(2026, 10, 16, 09, 0, 0, DateTimeKind.Utc),
            Updated = new DateTime(2026, 10, 16, 09, 0, 0, DateTimeKind.Utc),
            ETag = "etag",
        };
        MockIuuNotificationService
            .Insert(Arg.Any<IuuNotificationEntity>(), Arg.Any<CancellationToken>())
            .Returns(entity);

        var response = await client.PutAsJsonAsync(
            Testing.Endpoints.IuuNotifications.Put(IuuReference),
            iuuNotification
        );

        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    [Fact]
    public async Task Put_WhenEntityNotFound_ShouldBeNotFound()
    {
        var client = CreateClient();
        MockIuuNotificationService
            .Insert(Arg.Any<IuuNotificationEntity>(), Arg.Any<CancellationToken>())
            .Throws(new EntityNotFoundException("entityType", "entityId"));

        var response = await client.PutAsJsonAsync(
            Testing.Endpoints.IuuNotifications.Put(IuuReference),
            CreateIuuNotification()
        );

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Put_WhenConcurrencyException_ShouldBeConflict()
    {
        var client = CreateClient();
        MockIuuNotificationService
            .Insert(Arg.Any<IuuNotificationEntity>(), Arg.Any<CancellationToken>())
            .Throws(new ConcurrencyException("entityId", "etag"));

        var response = await client.PutAsJsonAsync(
            Testing.Endpoints.IuuNotifications.Put(IuuReference),
            CreateIuuNotification()
        );

        response.StatusCode.Should().Be(HttpStatusCode.Conflict);
    }
}
