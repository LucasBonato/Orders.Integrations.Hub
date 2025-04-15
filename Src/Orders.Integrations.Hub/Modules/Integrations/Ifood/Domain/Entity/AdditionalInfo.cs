using System.Text.Json.Serialization;

namespace Orders.Integrations.Hub.Modules.Integrations.Ifood.Domain.Entity;

public record AdditionalInfo(
    [property: JsonPropertyName("metadata")] Dictionary<string, string> Metadata
);