using System.Net;
using Defra.TradeImportsDataApi.Api.Data;
using Defra.TradeImportsDataApi.Api.Services;
using Defra.TradeImportsDataApi.Data.Entities;
using Defra.TradeImportsDataApi.Testing;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using WireMock.Server;
using Xunit.Abstractions;

namespace Defra.TradeImportsDataApi.Api.Tests.Endpoints.ResourceEvents;

public class PutTests : EndpointTestBase, IClassFixture<WireMockContext>
{
    private IResourceEventRepository MockResourceEventRepository { get; } = Substitute.For<IResourceEventRepository>();
    private IResourceEventService MockResourceEventService { get; } = Substitute.For<IResourceEventService>();
    private WireMockServer WireMock { get; }
    private const string ResourceId = "resourceId";

    public PutTests(ApiWebApplicationFactory factory, ITestOutputHelper outputHelper, WireMockContext context)
        : base(factory, outputHelper)
    {
        WireMock = context.Server;
        WireMock.Reset();
    }

    protected override void ConfigureTestServices(IServiceCollection services)
    {
        base.ConfigureTestServices(services);

        services.AddTransient<IResourceEventRepository>(_ => MockResourceEventRepository);
        services.AddTransient<IResourceEventService>(_ => MockResourceEventService);
    }

    [Fact]
    public async Task Published_WhenNotFound_ShouldBeNotFound()
    {
        var client = CreateClient();
        MockResourceEventRepository
            .GetAll(ResourceId, Arg.Any<CancellationToken>())
            .Returns(
                [
                    new ResourceEventEntity
                    {
                        Id = "id1",
                        ResourceId = "resourceId",
                        ResourceType = "resourceType",
                        Operation = "operation",
                        Message = "message",
                    },
                ]
            );

        var response = await client.PutAsync(
            TradeImportsDataApi.Testing.Endpoints.ResourceEvents.Publish(ResourceId, "unknown-id"),
            null
        );
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Published_WhenAlreadyPublished_ShouldBeConflict()
    {
        var client = CreateClient();
        MockResourceEventRepository
            .GetAll(ResourceId, Arg.Any<CancellationToken>())
            .Returns(
                [
                    new ResourceEventEntity
                    {
                        Id = "id1",
                        ResourceId = "resourceId",
                        ResourceType = "resourceType",
                        Operation = "operation",
                        Message = "message",
                        Published = DateTime.UtcNow,
                    },
                ]
            );

        var response = await client.PutAsync(
            TradeImportsDataApi.Testing.Endpoints.ResourceEvents.Publish(ResourceId, "id1"),
            null
        );
        response.StatusCode.Should().Be(HttpStatusCode.Conflict);
    }

    [Theory]
    [InlineData(null)]
    [InlineData(false)]
    public async Task Published_WhenNotPublished_ShouldBePublished(bool? force)
    {
        var client = CreateClient();
        var entity = new ResourceEventEntity
        {
            Id = "id1",
            ResourceId = "resourceId",
            ResourceType = "resourceType",
            Operation = "operation",
            Message = "message",
        };
        MockResourceEventRepository.GetAll(ResourceId, Arg.Any<CancellationToken>()).Returns([entity]);

        var response = await client.PutAsync(
            TradeImportsDataApi.Testing.Endpoints.ResourceEvents.Publish(ResourceId, "id1", force),
            null
        );
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        await MockResourceEventService.Received().PublishAllowException(entity, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Published_WhenAlreadyPublished_AndForced_ShouldBePublished()
    {
        var client = CreateClient();
        var entity = new ResourceEventEntity
        {
            Id = "id1",
            ResourceId = "resourceId",
            ResourceType = "resourceType",
            Operation = "operation",
            Message = "message",
            Published = DateTime.UtcNow,
        };
        MockResourceEventRepository.GetAll(ResourceId, Arg.Any<CancellationToken>()).Returns([entity]);

        var response = await client.PutAsync(
            TradeImportsDataApi.Testing.Endpoints.ResourceEvents.Publish(ResourceId, "id1", force: true),
            null
        );
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        await MockResourceEventService.Received().PublishAllowException(entity, Arg.Any<CancellationToken>());
    }
}
