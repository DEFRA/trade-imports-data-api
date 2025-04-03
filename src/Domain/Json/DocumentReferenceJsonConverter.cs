using System.Text.Json;
using System.Text.Json.Serialization;
using Defra.TradeImportsDataApi.Domain.CustomsDeclaration.ClearanceRequest;

namespace Defra.TradeImportsDataApi.Domain.Json;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class DocumentReferenceJsonConverterAttribute : JsonConverterAttribute
{
    public override JsonConverter? CreateConverter(Type typeToConvert)
    {
        return new DocumentReferenceJsonConverter();
    }
}

public class DocumentReferenceJsonConverter : JsonConverter<DocumentReference?>
{
    public override DocumentReference? Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        var value = reader.GetString();

        if (string.IsNullOrEmpty(value))
        {
            return null;
        }
        return new DocumentReference(value);
    }

    public override void Write(Utf8JsonWriter writer, DocumentReference? value, JsonSerializerOptions options)
    {
        if (value is not null)
        {
            writer.WriteStringValue(value!.Value);
        }
    }
}
