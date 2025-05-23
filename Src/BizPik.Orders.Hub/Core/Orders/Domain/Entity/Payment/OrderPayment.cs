using System.Text.Json.Serialization;

namespace BizPik.Orders.Hub.Core.Orders.Domain.Entity.Payment;

public record OrderPayment(
    [property: JsonPropertyName("prepaid")] int Prepaid,
    [property: JsonPropertyName("pending")] decimal Pending,
    [property: JsonPropertyName("methods")] IReadOnlyList<OrderPaymentMethod> Methods
);