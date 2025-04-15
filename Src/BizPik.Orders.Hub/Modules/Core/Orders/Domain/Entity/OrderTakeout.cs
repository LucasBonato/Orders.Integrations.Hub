using System.Text.Json.Serialization;

using BizPik.Orders.Hub.Modules.Core.Orders.Domain.ValueObjects.Enums;

namespace BizPik.Orders.Hub.Modules.Core.Orders.Domain.Entity;

public record OrderTakeout(
    [property: JsonPropertyName("mode")] OrderTakeoutMode Mode,
    [property: JsonPropertyName("takeoutDateTime")] DateTime TakeoutDateTime
);