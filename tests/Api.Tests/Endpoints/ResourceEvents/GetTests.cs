using System.Net;
using Defra.TradeImportsDataApi.Api.Data;
using Defra.TradeImportsDataApi.Data.Entities;
using Defra.TradeImportsDataApi.Testing;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using WireMock.Server;
using Xunit.Abstractions;

namespace Defra.TradeImportsDataApi.Api.Tests.Endpoints.ResourceEvents;

public class GetTests : EndpointTestBase, IClassFixture<WireMockContext>
{
    private IResourceEventRepository MockResourceEventRepository { get; } = Substitute.For<IResourceEventRepository>();
    private WireMockServer WireMock { get; }
    private const string ResourceId = "resourceId";
    private readonly VerifySettings _settings;

    public GetTests(ApiWebApplicationFactory factory, ITestOutputHelper outputHelper, WireMockContext context)
        : base(factory, outputHelper)
    {
        WireMock = context.Server;
        WireMock.Reset();

        _settings = new VerifySettings();
        _settings.ScrubMember("traceId");
        _settings.DontScrubDateTimes();
        _settings.DontScrubGuids();
        _settings.DontIgnoreEmptyCollections();
    }

    protected override void ConfigureTestServices(IServiceCollection services)
    {
        base.ConfigureTestServices(services);

        services.AddTransient<IResourceEventRepository>(_ => MockResourceEventRepository);
    }

    [Fact]
    public async Task GetAll_WhenNoneFound_ShouldBeOk()
    {
        var client = CreateClient();
        MockResourceEventRepository.GetAll(ResourceId, Arg.Any<CancellationToken>()).Returns([]);

        var response = await client.GetAsync(TradeImportsDataApi.Testing.Endpoints.ResourceEvents.GetAll(ResourceId));
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        await VerifyJson(await response.Content.ReadAsStringAsync(), _settings);
    }

    [Fact]
    public async Task GetAll_WhenFound_ShouldBeAsList()
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
                    new ResourceEventEntity
                    {
                        Id = "id2",
                        ResourceId = "resourceId2",
                        ResourceType = "resourceType2",
                        Operation = "operation2",
                        Message = "message2",
                    },
                ]
            );

        var response = await client.GetAsync(TradeImportsDataApi.Testing.Endpoints.ResourceEvents.GetAll(ResourceId));
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        await VerifyJson(await response.Content.ReadAsStringAsync(), _settings);
    }

    [Fact]
    public async Task Unpublished_WhenFound_ShouldBeAsList()
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
                        Published = DateTime.UtcNow, // Should not be in response as already published
                    },
                    new ResourceEventEntity
                    {
                        Id = "id2",
                        ResourceId = "resourceId2",
                        ResourceType = "resourceType2",
                        Operation = "operation2",
                        Message = "message2",
                    },
                ]
            );

        var response = await client.GetAsync(
            TradeImportsDataApi.Testing.Endpoints.ResourceEvents.Unpublished(ResourceId)
        );
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        await VerifyJson(await response.Content.ReadAsStringAsync(), _settings);
    }

    [Fact]
    public async Task Message_WhenFound_ShouldBeAsList()
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
                    new ResourceEventEntity
                    {
                        Id = "id2",
                        ResourceId = "resourceId2",
                        ResourceType = "resourceType2",
                        Operation = "operation2",
                        Message = "{\"resourceId\":\"resourceId2\"}",
                    },
                ]
            );

        var response = await client.GetAsync(
            TradeImportsDataApi.Testing.Endpoints.ResourceEvents.Message(ResourceId, "id2")
        );
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        await VerifyJson(await response.Content.ReadAsStringAsync(), _settings);
    }

    [Fact]
    public async Task Message_WhenNotFound_ShouldBeNotFound()
    {
        var client = CreateClient();
        MockResourceEventRepository.GetAll(ResourceId, Arg.Any<CancellationToken>()).Returns([]);

        var response = await client.GetAsync(
            TradeImportsDataApi.Testing.Endpoints.ResourceEvents.Message(ResourceId, "id2")
        );
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}
