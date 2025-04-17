using System.Text.Json.Serialization;

using BizPik.Orders.Hub.Modules.Core.Orders.Domain.ValueObjects.Enums;

namespace BizPik.Orders.Hub.Modules.Core.BizPik.Domain.ValueObjects;

public record BizPikSNSProductEvent(
    [property: JsonPropertyName("CompanyId")] int CompanyId,
    [property: JsonPropertyName("MerchantId")] string MerchantId,
    [property: JsonPropertyName("integration")] [property: JsonConverter(typeof(JsonStringEnumConverter))] OrderIntegration Integration,
    [property: JsonPropertyName("ProductsSkus")] List<string> ProductSkus
);