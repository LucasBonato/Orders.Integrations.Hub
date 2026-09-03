namespace Orders.Integrations.Hub.IntegrationTests.Contracts;

/// <summary>
/// Composed contract per integration: how the integration is identified (descriptor),
/// how webhooks are signed and where they land (webhook contact), and which payload
/// fixtures it supports (payload catalog).
/// </summary>
public interface IIntegrationContract
{
    IntegrationDescriptor Descriptor { get; }

    WebhookContact Webhook { get; }

    PayloadCatalog Payloads { get; }
}