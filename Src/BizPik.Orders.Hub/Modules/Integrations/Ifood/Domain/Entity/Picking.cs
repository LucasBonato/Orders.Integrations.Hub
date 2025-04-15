using System.Text.Json.Serialization;

using BizPik.Orders.Hub.Modules.Integrations.Ifood.Domain.ValueObjects.Enums;

namespace BizPik.Orders.Hub.Modules.Integrations.Ifood.Domain.Entity;

public record Picking(
    [property: JsonPropertyName("picker")] string Picker,
    [property: JsonPropertyName("replacementOptions")] ReplacementOptions ReplacementOptions
);