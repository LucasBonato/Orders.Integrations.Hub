using System.Text.Json.Serialization;

using BizPik.Orders.Hub.Modules.Common.Orders.Domain.ValueObjects.Enums;

namespace BizPik.Orders.Hub.Modules.Common.Orders.Domain.Entity;

public record OrderTakeout(
    [property: JsonPropertyName("mode")] OrderTakeoutMode Mode,
    [property: JsonPropertyName("takeoutDateTime")] DateTime TakeoutDateTime
);