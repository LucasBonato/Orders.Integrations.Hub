using System.Text.Json.Serialization;

namespace BizPik.Orders.Hub.Modules.Integrations.Ifood.Domain.Entity.Payments;

public record Card(
    [property: JsonPropertyName("brand")] string Brand
);