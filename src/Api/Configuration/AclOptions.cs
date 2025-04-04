using System.ComponentModel.DataAnnotations;

namespace Defra.TradeImportsDataApi.Api.Configuration;

public class AclOptions
{
    public Dictionary<string, Client> Clients { get; init; } = new();

    public class Client
    {
        [Required]
        public required string Secret { get; init; } = string.Empty;

        [Required]
        public required string[] Scopes { get; init; } = [];
    }
}
