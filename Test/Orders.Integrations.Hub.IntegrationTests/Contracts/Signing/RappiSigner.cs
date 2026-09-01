using System.Security.Cryptography;
using System.Text;

using Orders.Integrations.Hub.Integrations.Rappi.Application.Handlers;

namespace Orders.Integrations.Hub.IntegrationTests.Contracts.Signing;

public sealed class RappiSigner : IWebhookSigner
{
    public string HeaderName => "Rappi-Signature";

    public string Compute(string body, string secret)
    {
        RappiJsonSerializer serializer = new();
        string reserialized = serializer.Serialize(serializer.Deserialize<object>(body));
        string timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString();
        string payloadToSign = $"{timestamp}.{reserialized}";
        byte[] hash = HMACSHA256.HashData(
            Encoding.UTF8.GetBytes(secret),
            Encoding.UTF8.GetBytes(payloadToSign)
        );
        return $"t={timestamp},sign={Convert.ToHexStringLower(hash)}";
    }
}