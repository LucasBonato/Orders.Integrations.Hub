using System.Text.Json.Serialization;

namespace Orders.Integrations.Hub.Modules.Integrations.Ifood.Domain.Entity.Payments;

public record Wallet(
    [property: JsonPropertyName("name")] string Name
);