using System.Text.Json.Serialization;

namespace BizPik.Orders.Hub.Modules.Common.Orders.Adapter.Dtos.Entity.Merchant;

public record OrderMerchant(
    [property: JsonPropertyName("id")] string Id,
    [property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("address")] Address? Address
);