using System.Security.Cryptography;
using System.Text;

namespace Orders.Integrations.Hub.IntegrationTests.Contracts.Signing;

public sealed class IFoodSigner : IWebhookSigner
{
    public string HeaderName => "X-IFood-Signature";

    public string Compute(string body, string secret)
    {
        byte[] hash = HMACSHA256.HashData(
            Encoding.UTF8.GetBytes(secret),
            Encoding.UTF8.GetBytes(body)
        );
        return Convert.ToHexStringLower(hash);
    }
}