using Orders.Integrations.Hub.TestCommon.Configuration;
using Orders.Integrations.Hub.TestCommon.Signing;

namespace Orders.Integrations.Hub.TestCommon.Contracts.Food99;

public sealed class Food99Contract : IIntegrationContract
{
    public static readonly Food99Contract Instance = new();

    public IntegrationDescriptor Descriptor { get; } = new("Food99", "FOOD99");

    public WebhookContact Webhook { get; } = new(
        Signer: new Food99Signer(),
        ValidSecret: TestConfiguration.GetRequired("Integrations:Food99:Client:Secret"),
        SignatureRoute: "/api/v1/orders-hub/food99/webhook"
    );

    public PayloadCatalog Payloads { get; } = new("order-new");
}
