using System.Text.Json.Serialization;

namespace Orders.Integrations.Hub.Modules.Integrations.Ifood.Domain.Entity.Payments;

public record Card(
    [property: JsonPropertyName("brand")] string Brand
);