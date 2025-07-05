using System.Text.Json.Serialization;

namespace Orders.Integrations.Hub.Integrations.Ifood.Domain.Entity.Order.Payments;

public record Wallet(
    [property: JsonPropertyName("name")] string Name
);