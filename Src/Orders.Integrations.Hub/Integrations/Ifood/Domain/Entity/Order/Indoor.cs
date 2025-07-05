using System.Text.Json.Serialization;

using Orders.Integrations.Hub.Integrations.Ifood.Domain.ValueObjects.Enums;

namespace Orders.Integrations.Hub.Integrations.Ifood.Domain.Entity.Order;

public record Indoor(
    [property: JsonPropertyName("mode")] [property: JsonConverter(typeof(JsonStringEnumConverter))] IndoorMode Mode,
    [property: JsonPropertyName("table")] string Table,
    [property: JsonPropertyName("deliveryDateTime")] DateTime DeliveryDateTime,
    [property: JsonPropertyName("observations")] string Observations
);