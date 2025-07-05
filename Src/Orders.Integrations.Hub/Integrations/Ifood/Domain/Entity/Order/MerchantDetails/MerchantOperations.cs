using System.Text.Json.Serialization;

namespace Orders.Integrations.Hub.Integrations.Ifood.Domain.Entity.Order.MerchantDetails;

public record MerchantOperations(
    [property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("salesChannel")] MerchantSalesChannel SalesChannel
);