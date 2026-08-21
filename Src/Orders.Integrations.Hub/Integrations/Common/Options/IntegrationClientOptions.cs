namespace Orders.Integrations.Hub.Integrations.Common.Options;

public sealed class IntegrationClientOptions {
    public required Uri BaseUrl { get; init; }
    public Uri? AuthUrl { get; init; }
}