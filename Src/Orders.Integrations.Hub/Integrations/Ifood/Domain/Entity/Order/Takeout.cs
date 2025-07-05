using System.Text.Json.Serialization;

using Orders.Integrations.Hub.Integrations.Ifood.Domain.ValueObjects.Enums;

namespace Orders.Integrations.Hub.Integrations.Ifood.Domain.Entity.Order;

public record Takeout(
    [property: JsonPropertyName("mode")] [property: JsonConverter(typeof(JsonStringEnumConverter))] TakeoutMode Mode,
    [property: JsonPropertyName("takeoutDateTime")] DateTime TakeoutDateTime,
    [property: JsonPropertyName("observations")] string Observations
);