using Orders.Integrations.Hub.IntegrationTests.Contracts.Signing;

namespace Orders.Integrations.Hub.IntegrationTests.Contracts.Rappi;

public sealed class RappiContract : IIntegrationContract
{
    public static readonly RappiContract Instance = new();

    public IntegrationDescriptor Descriptor { get; } = new("Rappi", "RAPPI");

    public WebhookContact Webhook { get; } = new(
        Signer: new RappiSigner(),
        ValidSecret: "test-rappi-secret",
        SignatureRoute: "/api/v1/orders-hub/rappi/webhook"
    );

    public PayloadCatalog Payloads { get; } = new("create");
}
