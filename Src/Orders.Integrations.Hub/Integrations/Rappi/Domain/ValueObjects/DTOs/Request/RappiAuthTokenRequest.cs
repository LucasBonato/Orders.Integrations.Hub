namespace Orders.Integrations.Hub.Integrations.Rappi.Domain.ValueObjects.DTOs.Request;

public record RappiAuthTokenRequest(
    string ClienteId,
    string ClienteSecret,
    string Audience,
    string GrantType = "client_credentials"
);