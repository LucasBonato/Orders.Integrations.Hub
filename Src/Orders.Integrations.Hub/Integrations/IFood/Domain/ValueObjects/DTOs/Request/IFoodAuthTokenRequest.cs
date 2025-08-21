namespace Orders.Integrations.Hub.Integrations.IFood.Domain.ValueObjects.DTOs.Request;

public record IFoodAuthTokenRequest(
    string GrantType,
    string ClientId,
    string ClientSecret
);