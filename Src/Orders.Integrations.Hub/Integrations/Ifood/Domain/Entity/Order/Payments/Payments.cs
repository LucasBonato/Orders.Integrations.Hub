using System.Text.Json.Serialization;

namespace Orders.Integrations.Hub.Integrations.Ifood.Domain.Entity.Order.Payments;

public record Payments(
    [property: JsonPropertyName("prepaid")] decimal Prepaid,
    [property: JsonPropertyName("pending")] decimal Pending,
    [property: JsonPropertyName("methods")] IReadOnlyList<PaymentsMethod> Methods
);