using System.Text.Json.Serialization;

namespace BizPik.Orders.Hub.Modules.Integrations.Ifood.Domain.Entity.AdditionalFee;

public record Liability(
    [property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("percentage")] decimal Percentage
);