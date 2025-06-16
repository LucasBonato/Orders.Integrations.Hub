using System.Text.Json.Serialization;

using BizPik.Orders.Hub.Core.Domain.ValueObjects.Enums;

namespace BizPik.Orders.Hub.Core.Domain.ValueObjects.DTOs.BizPik;

public record BizPikSNSProductEvent(
    [property: JsonPropertyName("CompanyId")] int CompanyId,
    [property: JsonPropertyName("MerchantId")] string MerchantId,
    [property: JsonPropertyName("integration")] [property: JsonConverter(typeof(JsonStringEnumConverter))] OrderIntegration Integration,
    [property: JsonPropertyName("ProductsSkus")] List<string> ProductSkus
);