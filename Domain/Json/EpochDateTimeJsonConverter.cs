using System.Diagnostics;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Defra.TradeImportsData.Domain.Json;


[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class EpochDateTimeJsonConverterAttribute : JsonConverterAttribute
{
    public override JsonConverter? CreateConverter(Type typeToConvert)
    {
        return new EpochDateTimeJsonConverter();
    }
}

public class EpochDateTimeJsonConverter : JsonConverter<DateTime>
{
    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        Debug.Assert(typeToConvert == typeof(DateTime));

        ulong number = 0;

        if (reader.TokenType == JsonTokenType.Number)
        {
            reader.TryGetUInt64(out number);
        }
        else
        {
            var s = reader.GetString();
            if (!ulong.TryParse(s, out number))
            {
                return DateTime.Parse(s!, CultureInfo.CurrentCulture);
            }
        }

        var epoch = DateTime.UnixEpoch;

        // 1723127967 - DEV
        // 1712851200000 - SND
        if (number > 10000000000)
        {
            return epoch.AddMilliseconds(number);
        }
        else if (number > 0)
        {
            return epoch.AddSeconds(number);
        }

        return epoch;
    }

    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
    {
        writer.WriteNumberValue(new DateTimeOffset(value).ToUnixTimeMilliseconds());
    }
}