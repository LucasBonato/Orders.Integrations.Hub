using System.Security.Cryptography;
using System.Text;

using Orders.Integrations.Hub.Integrations.Common.Contracts;

namespace Orders.Integrations.Hub.Integrations.Common.Validators;

public class HmacSha256SignatureValidator : ISignatureValidator
{
    public bool IsValid(string signature, byte[] rawBody, string secret)
    {
        using HMACSHA256 hmac = new(Encoding.UTF8.GetBytes(secret));
        byte[] computedHash = hmac.ComputeHash(rawBody);

        if (!TryParseHex(signature, out byte[] expectedBytes))
            return false;

        return CryptographicOperations.FixedTimeEquals(computedHash, expectedBytes);
    }

    public bool IsValid(string signature, string body, string secret)
        => IsValid(signature, Encoding.UTF8.GetBytes(body), secret);

    private static bool TryParseHex(string hex, out byte[] bytes)
    {
        try { bytes = Convert.FromHexString(hex); return true; }
        catch { bytes = []; return false; }
    }
}