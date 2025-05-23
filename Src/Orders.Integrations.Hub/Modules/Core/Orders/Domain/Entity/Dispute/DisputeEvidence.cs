using System.Text.Json.Serialization;

namespace Orders.Integrations.Hub.Modules.Core.Orders.Domain.Entity.Dispute;

public record DisputeEvidence(
    [property: JsonPropertyName("url")] string Url
);