using System.Text.Json;
using Defra.TradeImportsDataApi.Domain.Json;
using FluentAssertions;

namespace Defra.TradeImportsDataApi.Api.Tests.Json;

public class EpochDateTimeJsonConverterTests
{
    [Theory]
    [InlineData("2025-05-08T14:21:50.286Z", DateTimeKind.Utc)]
    [InlineData("2025-05-08T14:21:50.286", DateTimeKind.Unspecified)]
    public void DateIfConvertedToCorrectKind(string date, DateTimeKind expected)
    {
        var result = EpochDateTimeJsonConverter.DateTimeFromString(date);

        result.Kind.Should().Be(expected);
    }

    [Theory]
    [InlineData("{\"TestDate\":\"2025-05-08T14:21:50.286Z\"}", DateTimeKind.Utc)]
    [InlineData("{\"TestDate\":\"2025-05-08T14:21:50.286\"}", DateTimeKind.Unspecified)]
    public void DeserializeTests(string json, DateTimeKind expected)
    {
        var result = JsonSerializer.Deserialize<TestObject>(json);

        result?.TestDate.Kind.Should().Be(expected);
    }

    public class TestObject
    {
        [EpochDateTimeJsonConverter]
        public DateTime TestDate { get; set; }
    }
}
