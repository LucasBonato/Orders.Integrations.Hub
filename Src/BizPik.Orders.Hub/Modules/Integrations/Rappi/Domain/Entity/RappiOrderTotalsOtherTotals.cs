using System.Text.Json.Serialization;

namespace BizPik.Orders.Hub.Modules.Integrations.Rappi.Domain.Entity;

public record RappiOrderTotalsOtherTotals(
    [property: JsonPropertyName("total_rappi_credits")] decimal? TotalRappiCredits,
    [property: JsonPropertyName("total_rappi_pay")] decimal? TotalRappiPay,
    [property: JsonPropertyName("tip")] decimal? Tip
);