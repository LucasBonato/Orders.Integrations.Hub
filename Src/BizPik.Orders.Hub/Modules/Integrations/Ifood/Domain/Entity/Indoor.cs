using System.Text.Json.Serialization;

using BizPik.Orders.Hub.Modules.Integrations.Ifood.Domain.ValueObjects.Enums;

namespace BizPik.Orders.Hub.Modules.Integrations.Ifood.Domain.Entity;

public record Indoor(
    [property: JsonPropertyName("mode")] IndoorMode Mode,
    [property: JsonPropertyName("table")] string Table,
    [property: JsonPropertyName("deliveryDateTime")] DateTime DeliveryDateTime,
    [property: JsonPropertyName("observations")] string Observations
);