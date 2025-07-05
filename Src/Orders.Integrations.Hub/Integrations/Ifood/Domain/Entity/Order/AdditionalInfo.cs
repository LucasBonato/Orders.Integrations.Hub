using System.Text.Json.Serialization;

namespace Orders.Integrations.Hub.Integrations.Ifood.Domain.Entity.Order;

public record AdditionalInfo(
    [property: JsonPropertyName("metadata")] Dictionary<string, string> Metadata
);