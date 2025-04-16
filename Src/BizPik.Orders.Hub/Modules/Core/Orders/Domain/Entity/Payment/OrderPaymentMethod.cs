using System.Text.Json.Serialization;

using BizPik.Orders.Hub.Modules.Core.Orders.Domain.ValueObjects.Enums;

namespace BizPik.Orders.Hub.Modules.Core.Orders.Domain.Entity.Payment;

public record OrderPaymentMethod(
    [property: JsonPropertyName("value")] decimal Value,
    [property: JsonPropertyName("currency")] string Currency,
    [property: JsonPropertyName("type")] MethodType Type,
    [property: JsonPropertyName("method")] MethodMethod Method,
    [property: JsonPropertyName("brand")] MethodBrand Brand,
    [property: JsonPropertyName("methodInfo")] string MethodInfo,
    [property: JsonPropertyName("transaction")] OrderPaymentMethodTransaction? Transaction,
    [property: JsonPropertyName("changeFor")] decimal? ChangeFor
);