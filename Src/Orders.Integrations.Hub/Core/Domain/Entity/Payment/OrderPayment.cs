using System.Text.Json.Serialization;

namespace Orders.Integrations.Hub.Core.Domain.Entity.Payment;

public record OrderPayment(
    [property: JsonPropertyName("prepaid")] int Prepaid,
    [property: JsonPropertyName("pending")] decimal Pending,
    [property: JsonPropertyName("methods")] IReadOnlyList<OrderPaymentMethod> Methods
);