using System.Text.Json.Serialization;

namespace BizPik.Orders.Hub.Modules.Common.Orders.Adapter.Dtos.Entity.Payment;

public record OrderPaymentMethodTransaction(
    [property: JsonPropertyName("authorizationCode")] string AuthorizationCode,
    [property: JsonPropertyName("acquirerDocument")] string AcquirerDocument
);