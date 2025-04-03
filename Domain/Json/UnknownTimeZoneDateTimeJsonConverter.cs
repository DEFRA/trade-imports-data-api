using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Defra.TradeImportsData.Domain.Json;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class UnknownTimeZoneDateTimeJsonConverterAttribute(string propertyName) : JsonConverterAttribute
{
    public override JsonConverter? CreateConverter(Type typeToConvert)
    {
        return new UnknownTimeZoneDateTimeJsonConverter(propertyName);
    }
}

public class UnknownTimeZoneDateTimeJsonConverter(string propertyName) : JsonConverter<DateTime>
{
    private const string JsonFormat = "yyyy-MM-ddTHH:mm:ss";

    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (typeToConvert != typeof(DateTime))
            throw new FormatException($"Invalid typeToConvert {typeToConvert.FullName} in {propertyName}");

        var dateTimeFromJson = reader.GetDateTime()!;

        if (dateTimeFromJson.Kind != DateTimeKind.Unspecified)
        {
            throw new FormatException($"Invalid Value in {propertyName}, value={reader.GetString()}. Unknown TimeZone dates must be DateTimeKind.Unspecified, not {dateTimeFromJson.Kind}");
        }

        return DateTime.SpecifyKind(dateTimeFromJson, DateTimeKind.Unspecified);
    }

    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
    {
        if (value.Kind != DateTimeKind.Unspecified)
        {
            throw new FormatException($"Invalid Value in {propertyName}, value={value.ToString("s", System.Globalization.CultureInfo.InvariantCulture)}. Unknown Timezone dates must be DateTimeKind.Unspecified, not {value.Kind}");
        }
        writer.WriteStringValue(value.ToString(JsonFormat, CultureInfo.InvariantCulture));
    }
}