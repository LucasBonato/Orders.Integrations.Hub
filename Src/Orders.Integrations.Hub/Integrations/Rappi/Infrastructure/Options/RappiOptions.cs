using Orders.Integrations.Hub.Integrations.Common.Options;

namespace Orders.Integrations.Hub.Integrations.Rappi.Infrastructure.Options;

public sealed class RappiOptions {
    public const string SectionName = "Integrations:Rappi";

    public required ClientCredentialsOptions Client { get; init; }
    public required IntegrationClientOptions Endpoint { get; init; }
}