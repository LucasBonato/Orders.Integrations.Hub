using System.Text.Json.Serialization;

namespace Orders.Integrations.Hub.Modules.Integrations.Ifood.Domain.Entity.Order.Payments;

public record Cash(
    [property: JsonPropertyName("changeFor")] decimal ChangeFor
);