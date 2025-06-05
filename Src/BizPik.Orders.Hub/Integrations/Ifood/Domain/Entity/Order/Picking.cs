using System.Text.Json.Serialization;

using BizPik.Orders.Hub.Integrations.Ifood.Domain.ValueObjects.Enums;

namespace BizPik.Orders.Hub.Integrations.Ifood.Domain.Entity.Order;

public record Picking(
    [property: JsonPropertyName("picker")] string Picker,
    [property: JsonPropertyName("replacementOptions")] [property: JsonConverter(typeof(JsonStringEnumConverter))] ReplacementOptions ReplacementOptions
);