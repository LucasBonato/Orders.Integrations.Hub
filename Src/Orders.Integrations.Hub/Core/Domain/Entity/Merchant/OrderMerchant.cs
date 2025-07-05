using System.Text.Json.Serialization;

namespace Orders.Integrations.Hub.Core.Domain.Entity.Merchant;

public record OrderMerchant(
    [property: JsonPropertyName("id")] string Id,
    [property: JsonPropertyName("name")] string Name
);