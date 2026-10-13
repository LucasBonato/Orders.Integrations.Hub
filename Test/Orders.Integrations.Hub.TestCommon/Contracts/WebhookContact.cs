using Orders.Integrations.Hub.TestCommon.Signing;

namespace Orders.Integrations.Hub.TestCommon.Contracts;

public sealed record WebhookContact(IWebhookSigner Signer, string ValidSecret, string SignatureRoute);