using System.Text.Json.Serialization;

namespace BizPik.Orders.Hub.Modules.Integrations.Ifood.Domain.Entity.MerchantDetails;

public record MerchantOperations(
    [property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("salesChannel")] MerchantSalesChannel SalesChannel
);