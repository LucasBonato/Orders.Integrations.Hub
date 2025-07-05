using System.Text.Json.Serialization;

namespace Orders.Integrations.Hub.Integrations.Rappi.Domain.Entity;

public record RappiOrder(
    [property: JsonPropertyName("order_detail")] RappiOrderDetails OrderDetail,
    [property: JsonPropertyName("customer")] RappiOrderCustomer? Customer,
    [property: JsonPropertyName("store")] RappiOrderStore Store
);