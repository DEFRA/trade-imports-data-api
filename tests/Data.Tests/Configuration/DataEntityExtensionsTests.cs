using Defra.TradeImportsDataApi.Data.Configuration;
using Defra.TradeImportsDataApi.Data.Entities;
using FluentAssertions;

namespace Defra.TradeImportsDataApi.Data.Tests.Configuration;

public class DataEntityExtensionsTests
{
    [Fact]
    public void NoAttributeTest()
    {
        typeof(NoAttributeClassEntity).DataEntityName().Should().Be("NoAttributeClass");
    }

    [Fact]
    public void AttributeTest()
    {
        typeof(AttributeClassEntity).DataEntityName().Should().Be("TestName");
    }

    public record NoAttributeClassEntity(string Test);

    [DbCollection("TestName")]
    public record AttributeClassEntity(string Test);
}
