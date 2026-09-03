using Orders.Integrations.Hub.IntegrationTests.Contracts.Signing;

namespace Orders.Integrations.Hub.IntegrationTests.Contracts;

public sealed record WebhookContact(IWebhookSigner Signer, string ValidSecret, string SignatureRoute);