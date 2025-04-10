using System.Text.Json.Serialization;

using BizPik.Orders.Hub.Modules.Common.Orders.Adapter.Dtos.Enums;

namespace BizPik.Orders.Hub.Modules.Common.Orders.Adapter.Dtos.Entity;

public record OrderTakeout(
    [property: JsonPropertyName("mode")] OrderTakeoutMode Mode,
    [property: JsonPropertyName("takeoutDateTime")] DateTime TakeoutDateTime
);