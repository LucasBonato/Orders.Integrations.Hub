using System.Text.Json.Serialization;

using Orders.Integrations.Hub.Core.Orders.Domain.ValueObjects.Enums;

namespace Orders.Integrations.Hub.Core.Orders.Domain.ValueObjects.DTOs.;

public record SNSProductEvent(
    [property: JsonPropertyName("CompanyId")] int CompanyId,
    [property: JsonPropertyName("MerchantId")] string MerchantId,
    [property: JsonPropertyName("integration")] [property: JsonConverter(typeof(JsonStringEnumConverter))] OrderIntegration Integration,
    [property: JsonPropertyName("ProductsSkus")] List<string> ProductSkus
);