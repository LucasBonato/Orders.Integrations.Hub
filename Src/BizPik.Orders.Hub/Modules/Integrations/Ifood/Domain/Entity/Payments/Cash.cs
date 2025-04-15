using System.Text.Json.Serialization;

namespace BizPik.Orders.Hub.Modules.Integrations.Ifood.Domain.Entity.Payments;

public record Cash(
    [property: JsonPropertyName("changeFor")] decimal ChangeFor
);