using System.Text.Json.Serialization;

using Orders.Integrations.Hub.Modules.Integrations.Ifood.Domain.ValueObjects.Enums;

namespace Orders.Integrations.Hub.Modules.Integrations.Ifood.Domain.Entity;

public record Indoor(
    [property: JsonPropertyName("mode")] IndoorMode Mode,
    [property: JsonPropertyName("table")] string Table,
    [property: JsonPropertyName("deliveryDateTime")] DateTime DeliveryDateTime,
    [property: JsonPropertyName("observations")] string Observations
);