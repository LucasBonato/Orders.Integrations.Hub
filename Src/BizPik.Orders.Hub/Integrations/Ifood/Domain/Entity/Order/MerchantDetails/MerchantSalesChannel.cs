using System.Text.Json.Serialization;

namespace BizPik.Orders.Hub.Integrations.Ifood.Domain.Entity.Order.MerchantDetails;

public record MerchantSalesChannel(
    [property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("enabled")] string Enabled
);