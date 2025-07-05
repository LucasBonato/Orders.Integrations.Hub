using System.Text.Json.Serialization;

namespace Orders.Integrations.Hub.Core.Domain.Entity.Dispute;

public record DisputeEvidence(
    [property: JsonPropertyName("url")] string Url
);