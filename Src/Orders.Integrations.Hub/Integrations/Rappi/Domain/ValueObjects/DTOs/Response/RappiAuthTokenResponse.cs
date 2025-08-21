namespace Orders.Integrations.Hub.Integrations.Rappi.Domain.ValueObjects.DTOs.Response;

public record RappiAuthTokenResponse(
    string AccessToken,
    string Scope,
    int ExpiresIn,
    string TokenType
);