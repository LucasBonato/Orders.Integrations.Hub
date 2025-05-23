using System.Text.Json.Serialization;

namespace Orders.Integrations.Hub.Modules.Integrations.Ifood.Domain.Entity.Order.MerchantDetails;

public record MerchantSalesChannel(
    [property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("enabled")] string Enabled
);