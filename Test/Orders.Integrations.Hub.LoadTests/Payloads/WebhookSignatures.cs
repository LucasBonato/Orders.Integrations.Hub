using Orders.Integrations.Hub.TestCommon.Signing;

namespace Orders.Integrations.Hub.LoadTests.Payloads;

/// <summary>
/// Computes webhook signature headers using the real TestCommon signers.
/// Secrets are read from appsettings.Test.json via the LoadTestHost configuration.
/// </summary>
public static class WebhookSignatures
{
    private static readonly IWebhookSigner IFoodSigner = new IFoodSigner();
    private static readonly IWebhookSigner RappiSigner = new RappiSigner();
    private static readonly IWebhookSigner Food99Signer = new Food99Signer();

    private const string IFoodSecret = "test-ifood-secret";
    private const string RappiSecret = "test-rappi-secret";
    private const string Food99Secret = "test-food99-secret";

    public static IReadOnlyDictionary<string, string> HeadersFor(string integrationKey, string rawJsonBody)
    {
        (IWebhookSigner signer, string secret) = integrationKey.ToUpperInvariant() switch
        {
            "IFOOD" => (IFoodSigner, IFoodSecret),
            "RAPPI" => (RappiSigner, RappiSecret),
            "FOOD99" => (Food99Signer, Food99Secret),
            _ => throw new NotSupportedException($"No signature implementation wired for '{integrationKey}'.")
        };

        string value = signer.Compute(rawJsonBody, secret);

        return new Dictionary<string, string>
        {
            [signer.HeaderName] = value
        };
    }
}
