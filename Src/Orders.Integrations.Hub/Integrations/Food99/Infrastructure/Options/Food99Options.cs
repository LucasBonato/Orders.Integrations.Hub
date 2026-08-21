using Orders.Integrations.Hub.Integrations.Common.Options;

namespace Orders.Integrations.Hub.Integrations.Food99.Infrastructure.Options;

public sealed class Food99Options {
    public const string SectionName = "Integrations:Food99";

    public required ClientCredentialsOptions Client { get; init; }
    public required IntegrationClientOptions Endpoint { get; init; }
}