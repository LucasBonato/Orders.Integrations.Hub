using Orders.Integrations.Hub.TestCommon.Configuration;
using Orders.Integrations.Hub.TestCommon.Signing;

namespace Orders.Integrations.Hub.TestCommon.Contracts.Rappi;

public sealed class RappiContract : IIntegrationContract
{
    public static readonly RappiContract Instance = new();

    public IntegrationDescriptor Descriptor { get; } = new("Rappi", "RAPPI");

    public WebhookContact Webhook { get; } = new(
        Signer: new RappiSigner(),
        ValidSecret: TestConfiguration.GetRequired("Integrations:Rappi:Client:Secret"),
        SignatureRoute: "/api/v1/orders-hub/rappi/webhook"
    );

    public PayloadCatalog Payloads { get; } = new("create");
}
