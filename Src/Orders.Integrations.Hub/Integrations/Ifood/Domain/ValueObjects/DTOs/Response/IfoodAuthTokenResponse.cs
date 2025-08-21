namespace Orders.Integrations.Hub.Integrations.Ifood.Domain.ValueObjects.DTOs.Response;

public record IfoodAuthTokenResponse(
    string AccessToken,
    string Type,
    int ExpiresIn
);