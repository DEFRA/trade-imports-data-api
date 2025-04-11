using Defra.TradeImportsDataApi.Api.Utils;
using FluentAssertions;

namespace Defra.TradeImportsDataApi.Api.Tests.Utils;

public class DiffTests
{
    public class SimpleClass
    {
        public required string Name { get; set; }
    }

    [Fact]
    public void WhenCurrentIsNull_ShouldThrow()
    {
        var entity = new SimpleClass { Name = "Test" };

        var act = () => DiffExtensions.CreateDiff(null!, entity);

        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void WhenPreviousIsNull_ShouldThrow()
    {
        var entity = new SimpleClass { Name = "Test" };

        var act = () => DiffExtensions.CreateDiff(entity, null!);

        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void WhenCurrentAndPreviousHasChanged_ShouldReturnDiff()
    {
        var current = new SimpleClass { Name = "Test" };
        var previous = new SimpleClass { Name = "Test_Changed" };

        var result = DiffExtensions.CreateDiff(current, previous);

        result.Count.Should().Be(1);
        result[0].Operation.Should().Be("Replace");
        result[0].Path.Should().Be("/Name");
        result[0].Value.Should().Be("Test");
    }
}
