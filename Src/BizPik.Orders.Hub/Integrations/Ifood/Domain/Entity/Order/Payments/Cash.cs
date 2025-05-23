using System.Text.Json.Serialization;

namespace BizPik.Orders.Hub.Integrations.Ifood.Domain.Entity.Order.Payments;

public record Cash(
    [property: JsonPropertyName("changeFor")] decimal ChangeFor
);