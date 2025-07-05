using System.Text.Json.Serialization;

using Orders.Integrations.Hub.Integrations.Ifood.Domain.ValueObjects.Enums;

namespace Orders.Integrations.Hub.Integrations.Ifood.Domain.Entity.Order.Payments;

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