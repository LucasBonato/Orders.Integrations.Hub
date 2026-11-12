using Orders.Integrations.Hub.TestCommon.Configuration;
using Orders.Integrations.Hub.TestCommon.Signing;

namespace Orders.Integrations.Hub.TestCommon.Contracts.IFood;

public sealed class IFoodContract : IIntegrationContract
{
    public static readonly IFoodContract Instance = new();

    public IntegrationDescriptor Descriptor { get; } = new("IFood", "IFOOD");

    public WebhookContact Webhook { get; } = new(
        Signer: new IFoodSigner(),
        ValidSecret: TestConfiguration.GetRequired("Integrations:IFood:Client:Secret"),
        SignatureRoute: "/api/v1/orders-hub/ifood/webhook"
    );

    public PayloadCatalog Payloads { get; } = new("keepalive");
}
