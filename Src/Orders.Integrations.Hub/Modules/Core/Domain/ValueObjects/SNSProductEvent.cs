using System.Text.Json.Serialization;

using Orders.Integrations.Hub.Modules.Core.Orders.Domain.ValueObjects.Enums;

namespace Orders.Integrations.Hub.Modules.Core..Domain.ValueObjects;

public record SNSProductEvent(
    [property: JsonPropertyName("CompanyId")] int CompanyId,
    [property: JsonPropertyName("MerchantId")] string MerchantId,
    [property: JsonPropertyName("integration")] [property: JsonConverter(typeof(JsonStringEnumConverter))] OrderIntegration Integration,
    [property: JsonPropertyName("ProductsSkus")] List<string> ProductSkus
);