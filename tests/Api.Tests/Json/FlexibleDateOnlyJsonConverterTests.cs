using System.Text.Json;
using Defra.TradeImportsDataApi.Domain.Json;
using FluentAssertions;

namespace Defra.TradeImportsDataApi.Api.Tests.Json;

public class FlexibleDateOnlyJsonConverterTests
{
    [Fact]
    public void Serialize_WritesIsoDate()
    {
        var json = JsonSerializer.Serialize(new TestObject { TestDate = new DateOnly(2025, 5, 8) });

        json.Should().Be("{\"TestDate\":\"2025-05-08\"}");
    }

    private class TestObject
    {
        [FlexibleDateOnlyJsonConverter]
        public DateOnly? TestDate { get; set; }
    }
}
