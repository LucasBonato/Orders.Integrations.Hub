using System.Text.Json.Serialization;

using BizPik.Orders.Hub.Integrations.Ifood.Domain.ValueObjects.Enums;

namespace BizPik.Orders.Hub.Integrations.Ifood.Domain.Entity.Order;

public record Indoor(
    [property: JsonPropertyName("mode")] [property: JsonConverter(typeof(JsonStringEnumConverter))] IndoorMode Mode,
    [property: JsonPropertyName("table")] string Table,
    [property: JsonPropertyName("deliveryDateTime")] DateTime DeliveryDateTime,
    [property: JsonPropertyName("observations")] string Observations
);