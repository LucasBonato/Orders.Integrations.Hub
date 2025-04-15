using System.Text.Json.Serialization;

using Orders.Integrations.Hub.Modules.Common.Orders.Domain.ValueObjects.Enums;

namespace Orders.Integrations.Hub.Modules.Common.Orders.Domain.Entity;

public record OrderTakeout(
    [property: JsonPropertyName("mode")] OrderTakeoutMode Mode,
    [property: JsonPropertyName("takeoutDateTime")] DateTime TakeoutDateTime
);