using Orders.Integrations.Hub.IntegrationTests.Contracts.Signing;

namespace Orders.Integrations.Hub.IntegrationTests.Contracts.IFood;

public sealed class IFoodContract : IIntegrationContract
{
    public static readonly IFoodContract Instance = new();

    public IntegrationDescriptor Descriptor { get; } = new("IFood", "IFOOD");

    public WebhookContact Webhook { get; } = new(
        Signer: new IFoodSigner(),
        ValidSecret: "test-secret",
        SignatureRoute: "/api/v1/orders-hub/ifood/webhook"
    );

    public PayloadCatalog Payloads { get; } = new("keepalive");
}
