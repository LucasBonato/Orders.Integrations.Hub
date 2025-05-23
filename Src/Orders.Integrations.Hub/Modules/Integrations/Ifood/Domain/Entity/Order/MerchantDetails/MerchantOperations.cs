using System.Text.Json.Serialization;

namespace Orders.Integrations.Hub.Modules.Integrations.Ifood.Domain.Entity.Order.MerchantDetails;

public record MerchantOperations(
    [property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("salesChannel")] MerchantSalesChannel SalesChannel
);