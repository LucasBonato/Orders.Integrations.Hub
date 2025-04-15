using System.Text.Json.Serialization;

namespace BizPik.Orders.Hub.Modules.Core.Orders.Domain.Entity.Payment;

public record OrderPaymentMethod(
    [property: JsonPropertyName("value")] decimal Value,
    [property: JsonPropertyName("currency")] string Currency,
    [property: JsonPropertyName("type")] string Type,
    [property: JsonPropertyName("method")] string Method,
    [property: JsonPropertyName("brand")] string Brand,
    [property: JsonPropertyName("methodInfo")] string MethodInfo,
    [property: JsonPropertyName("transaction")] OrderPaymentMethodTransaction? Transaction,
    [property: JsonPropertyName("changeFor")] decimal? ChangeFor
);