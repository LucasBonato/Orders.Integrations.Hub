using System.Text.Json.Serialization;

namespace Orders.Integrations.Hub.Integrations.Ifood.Domain.Entity.Order.Payments;

public record TransactionMethod(
    [property: JsonPropertyName("authorizationCode")] string AuthorizationCode,
    [property: JsonPropertyName("acquirerDocument")] string AcquirerDocument
);