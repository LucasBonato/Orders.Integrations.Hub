namespace Orders.Integrations.Hub.Integrations.IFood.Domain.ValueObjects.DTOs.Response;

public record IFoodAuthTokenResponse(
    string AccessToken,
    string Type,
    int ExpiresIn
);