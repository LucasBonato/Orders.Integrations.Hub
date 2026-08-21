using Orders.Integrations.Hub.Integrations.Common.Options;

namespace Orders.Integrations.Hub.Integrations.IFood.Infrastructure.Options;

public sealed class IFoodOptions {
    public const string SectionName = "Integrations:IFood";

    public required ClientCredentialsOptions Client { get; init; }
    public required IntegrationClientOptions Endpoint { get; init; }
}