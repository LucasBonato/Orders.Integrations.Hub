using System.Text.Json.Serialization;

namespace BizPik.Orders.Hub.Integrations.Ifood.Domain.Entity.Order;

public record AdditionalInfo(
    [property: JsonPropertyName("metadata")] Dictionary<string, string> Metadata
);