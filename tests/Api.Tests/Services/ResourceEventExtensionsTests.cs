using Defra.TradeImportsDataApi.Api.Services;
using Defra.TradeImportsDataApi.Data.Entities;
using FluentAssertions;

namespace Defra.TradeImportsDataApi.Api.Tests.Services;

public class ResourceEventExtensionsTests
{
    [Fact]
    public void WhenToResourceEvent_ShouldMap()
    {
        var subject = new FixtureEntity { Id = "id", ETag = "etag" };

        var result = subject.ToResourceEvent("operation");

        result.ResourceId.Should().Be("id");
        result.ResourceType.Should().Be("Fixture");
        result.Operation.Should().Be("operation");
        result.ETag.Should().Be("etag");
    }

    [Fact]
    public void WhenWithChangeSet_ShouldCreateChangeSet()
    {
        var previous = new FixtureEntity
        {
            Name = "From",
            Id = "id",
            ETag = "etag",
            FixtureType = FixtureType.Value1,
        };
        var current = new FixtureEntity
        {
            Name = "To",
            Id = "id",
            ETag = "etag",
            FixtureType = FixtureType.Value2,
        };
        var subject = current.ToResourceEvent("operation");

        var result = subject.WithChangeSet(current, previous);

        result.ChangeSet.Count.Should().Be(2);
        result.ChangeSet[0].Operation.Should().Be("Replace");
        result.ChangeSet[0].Path.Should().Be("/Name");
        result.ChangeSet[0].Value.Should().Be("To");
        result.ChangeSet[1].Operation.Should().Be("Replace");
        result.ChangeSet[1].Path.Should().Be("/FixtureType");
        result.ChangeSet[1].Value.Should().Be("Value2");
    }

    private class FixtureEntity : IDataEntity
    {
        public string Name { get; set; } = null!;
        public string Id { get; set; } = null!;
        public string ETag { get; set; } = null!;
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public FixtureType FixtureType { get; set; }

        public void OnSave() { }
    }

    private enum FixtureType
    {
        Value1,
        Value2,
    }
}
