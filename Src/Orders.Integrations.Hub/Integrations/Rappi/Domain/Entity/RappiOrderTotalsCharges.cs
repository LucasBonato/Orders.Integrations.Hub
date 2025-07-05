using System.Text.Json.Serialization;

namespace Orders.Integrations.Hub.Integrations.Rappi.Domain.Entity;

public record RappiOrderTotalsCharges(
    [property: JsonPropertyName("shipping")] decimal? Shipping,
    [property: JsonPropertyName("service_fee")] decimal? ServiceFee
);