using System.Text.Json.Serialization;

namespace BizPik.Orders.Hub.Modules.Integrations.Rappi.Domain.Entity;

public record RappiOrderTotalsCharges(
    [property: JsonPropertyName("shipping")] decimal? Shipping,
    [property: JsonPropertyName("service_fee")] decimal? ServiceFee
);