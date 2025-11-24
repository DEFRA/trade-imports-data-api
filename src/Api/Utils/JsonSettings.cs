using System.Text.Json;

namespace Defra.TradeImportsDataApi.Api.Utils;

public static class JsonSettings
{
    public static readonly JsonSerializerOptions Instance = new() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
}
