using Orders.Integrations.Hub.IntegrationTests.Contracts.Signing;

namespace Orders.Integrations.Hub.IntegrationTests.Contracts.Food99;

public sealed class Food99Contract : IIntegrationContract
{
    public static readonly Food99Contract Instance = new();

    public IntegrationDescriptor Descriptor { get; } = new("Food99", "FOOD99");

    public WebhookContact Webhook { get; } = new(
        Signer: new Food99Signer(),
        ValidSecret: "test-99-secret",
        SignatureRoute: "/api/v1/orders-hub/food99/webhook"
    );

    public PayloadCatalog Payloads { get; } = new("order-new");
}
