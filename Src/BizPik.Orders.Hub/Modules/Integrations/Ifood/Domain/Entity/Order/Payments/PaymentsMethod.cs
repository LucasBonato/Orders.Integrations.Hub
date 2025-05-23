using System.Text.Json.Serialization;

using BizPik.Orders.Hub.Modules.Integrations.Ifood.Domain.ValueObjects.Enums;

namespace BizPik.Orders.Hub.Modules.Integrations.Ifood.Domain.Entity.Order.Payments;

public record PaymentsMethod(
    [property: JsonPropertyName("value")] decimal Value,
    [property: JsonPropertyName("currency")] string Currency,
    [property: JsonPropertyName("type")] [property: JsonConverter(typeof(JsonStringEnumConverter))] IfoodMethodType Type,
    [property: JsonPropertyName("method")] [property: JsonConverter(typeof(JsonStringEnumConverter))] Method Method,
    [property: JsonPropertyName("wallet")] Wallet? Wallet,
    [property: JsonPropertyName("card")] Card? Card,
    [property: JsonPropertyName("cash")] Cash? Cash,
    [property: JsonPropertyName("transaction")] TransactionMethod? Transaction
);