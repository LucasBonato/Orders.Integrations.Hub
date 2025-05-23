using System.Text.Json.Serialization;

namespace BizPik.Orders.Hub.Integrations.Rappi.Domain.ValueObjects.DTOs;

public record RappiAvailabilityItem(
    [property: JsonPropertyName("turn_on")] List<string> TurnOn,
    [property: JsonPropertyName("turn_off")] List<string> TurnOff
);