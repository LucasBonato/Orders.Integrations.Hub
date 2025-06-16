using System.Text.Json.Serialization;

using Orders.Integrations.Hub.Core.Orders.Domain.ValueObjects.Enums;

namespace Orders.Integrations.Hub.Core.Orders.Domain.Entity;

public record OrderTakeout(
    [property: JsonPropertyName("mode")] [property: JsonConverter(typeof(JsonStringEnumConverter))] OrderTakeoutMode Mode,
    [property: JsonPropertyName("takeoutDateTime")] DateTime TakeoutDateTime
);