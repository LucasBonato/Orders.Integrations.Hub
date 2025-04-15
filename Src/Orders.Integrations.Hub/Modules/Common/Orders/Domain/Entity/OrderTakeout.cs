using System.Text.Json.Serialization;

using Orders.Integrations.Hub.Modules.Common.Orders.Adapter.Dtos.Enums;

namespace Orders.Integrations.Hub.Modules.Common.Orders.Adapter.Dtos.Entity;

public record OrderTakeout(
    [property: JsonPropertyName("mode")] OrderTakeoutMode Mode,
    [property: JsonPropertyName("takeoutDateTime")] DateTime TakeoutDateTime
);