using System.Text.Json.Serialization;

using BizPik.Orders.Hub.Core.Orders.Domain.ValueObjects.Enums;

namespace BizPik.Orders.Hub.Core.Orders.Domain.Entity;

public record OrderFee(
    [property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("type")] [property: JsonConverter(typeof(JsonStringEnumConverter))] FeeType Type,
    [property: JsonPropertyName("receivedBy")] [property: JsonConverter(typeof(JsonStringEnumConverter))] FeeReceivedBy ReceivedBy,
    [property: JsonPropertyName("price")] Price Price,
    [property: JsonPropertyName("receiverDocument")] string ReceiverDocument,
    [property: JsonPropertyName("observation")] string Observation
);