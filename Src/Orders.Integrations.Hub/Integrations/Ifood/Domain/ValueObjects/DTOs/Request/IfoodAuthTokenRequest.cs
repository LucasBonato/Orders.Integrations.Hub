namespace Orders.Integrations.Hub.Integrations.Ifood.Domain.ValueObjects.DTOs.Request;

public record IfoodAuthTokenRequest(
    string GrantType,
    string ClientId,
    string ClientSecret
);