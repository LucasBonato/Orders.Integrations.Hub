using System.Text.Json.Serialization;

namespace BizPik.Orders.Hub.Modules.Integrations.Ifood.Domain.Entity;

public record AdditionalInfo(
    [property: JsonPropertyName("metadata")] Dictionary<string, string> Metadata
);