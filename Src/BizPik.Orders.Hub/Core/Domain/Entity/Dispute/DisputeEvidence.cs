using System.Text.Json.Serialization;

namespace BizPik.Orders.Hub.Core.Orders.Domain.Entity.Dispute;

public record DisputeEvidence(
    [property: JsonPropertyName("url")] string Url
);