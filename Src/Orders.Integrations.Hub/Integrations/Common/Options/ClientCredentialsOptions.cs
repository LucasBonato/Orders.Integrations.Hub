namespace Orders.Integrations.Hub.Integrations.Common.Options;

public sealed class ClientCredentialsOptions {
    public required string Id { get; init; }
    public required string Secret { get; init; }
    public string? Audience { get; init; }
}