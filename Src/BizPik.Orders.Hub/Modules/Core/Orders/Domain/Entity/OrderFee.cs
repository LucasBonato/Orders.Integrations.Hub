using System.Text.Json.Serialization;

using BizPik.Orders.Hub.Modules.Core.Orders.Domain.ValueObjects.Enums;

namespace BizPik.Orders.Hub.Modules.Core.Orders.Domain.Entity;

public record OrderFee(
    [property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("type")] FeeType Type,
    [property: JsonPropertyName("receivedBy")] FeeReceivedBy ReceivedBy,
    [property: JsonPropertyName("price")] Price Price,
    [property: JsonPropertyName("receiverDocument")] string ReceiverDocument,
    [property: JsonPropertyName("observation")] string Observation
);