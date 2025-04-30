using System.Text.Json.Serialization;

namespace BizPik.Orders.Hub.Modules.Integrations.Ifood.Domain.Entity.MerchantDetails;

public record MerchantSalesChannel(
    [property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("enabled")] string Enabled
);