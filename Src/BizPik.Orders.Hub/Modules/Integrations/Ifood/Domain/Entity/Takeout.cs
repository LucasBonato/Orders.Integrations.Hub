using System.Text.Json.Serialization;

using BizPik.Orders.Hub.Modules.Integrations.Ifood.Domain.ValueObjects.Enums;

namespace BizPik.Orders.Hub.Modules.Integrations.Ifood.Domain.Entity;

public record Takeout(
    [property: JsonPropertyName("mode")] TakeoutMode mode,
    [property: JsonPropertyName("takeoutDateTime")] DateTime TakeoutDateTime,
    [property: JsonPropertyName("observations")] string Observations
);