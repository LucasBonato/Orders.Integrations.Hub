using System.Text.Json.Serialization;

namespace BizPik.Orders.Hub.Modules.Integrations.Ifood.Domain.Entity.Order.Payments;

public record Wallet(
    [property: JsonPropertyName("name")] string Name
);