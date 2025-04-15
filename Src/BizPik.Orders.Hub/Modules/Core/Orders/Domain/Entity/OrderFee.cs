using System.Text.Json.Serialization;

namespace BizPik.Orders.Hub.Modules.Core.Orders.Domain.Entity;

public record OrderFee(
    [property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("type")] string Type,
    [property: JsonPropertyName("receivedBy")] string ReceivedBy,
    [property: JsonPropertyName("price")] Price Price,
    [property: JsonPropertyName("receiverDocument")] string ReceiverDocument,
    [property: JsonPropertyName("observation")] string Observation,
    [property: JsonPropertyName("currency")] string Currency
);