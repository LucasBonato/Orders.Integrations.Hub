using System.Text.Json.Serialization;

namespace Orders.Integrations.Hub.Modules.Integrations.Ifood.Domain.Entity.MerchantDetails;

public record MerchantOperations(
    [property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("salesChannel")] MerchantSalesChannel SalesChannel
);