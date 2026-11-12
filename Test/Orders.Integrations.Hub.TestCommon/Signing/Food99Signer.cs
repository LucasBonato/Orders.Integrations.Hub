using System.Security.Cryptography;
using System.Text;

namespace Orders.Integrations.Hub.TestCommon.Signing;

public sealed class Food99Signer : IWebhookSigner
{
    public string HeaderName => "didi-header-sign";

    public string Compute(string body, string secret)
        => Convert.ToHexStringLower(MD5.HashData(Encoding.UTF8.GetBytes($"{body}{secret}")));
}
