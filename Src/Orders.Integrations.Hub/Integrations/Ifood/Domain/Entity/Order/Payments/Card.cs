using System.Text.Json.Serialization;

namespace Orders.Integrations.Hub.Integrations.Ifood.Domain.Entity.Order.Payments;

public record Card(
    [property: JsonPropertyName("brand")] string Brand
);