using System.Text.Json.Serialization;

namespace BizPik.Orders.Hub.Modules.Core.Orders.Domain.Entity.Payment;

public record OrderPayment(
    [property: JsonPropertyName("prepaid")] int Prepaid,
    [property: JsonPropertyName("pending")] double Pending,
    [property: JsonPropertyName("methods")] IReadOnlyList<OrderPaymentMethod> Methods
);