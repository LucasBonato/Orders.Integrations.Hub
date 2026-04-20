using System.Security.Cryptography;
using System.Text;

using Orders.Integrations.Hub.Integrations.Common.Contracts;

namespace Orders.Integrations.Hub.Integrations.Common.Validators;

public class Md5SignatureValidator : ISignatureValidator
{
    public bool IsValid(string signature, byte[] rawBody, string secret)
    {
        string body = Encoding.UTF8.GetString(rawBody);
        return IsValid(signature, body, secret);
    }

    public bool IsValid(string signature, string body, string secret)
    {
        byte[] inputBytes = Encoding.UTF8.GetBytes($"{body}{secret}");
        byte[] hashBytes = MD5.HashData(inputBytes);
        string computed = Convert.ToHexString(hashBytes).ToLowerInvariant();

        return string.Equals(computed, signature, StringComparison.OrdinalIgnoreCase);
    }
}