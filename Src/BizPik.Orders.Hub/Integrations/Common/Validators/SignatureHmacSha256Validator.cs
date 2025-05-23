using System.Security.Cryptography;
using System.Text;

namespace BizPik.Orders.Hub.Integrations.Common.Validators;

public static class SignatureHmacSha256Validator
{
    /// <summary>
    /// Compares the <c>expectedSignature</c> to a generated signature using a secret and some data.
    /// </summary>
    /// <param name="secret">The client secret of integrations</param>
    /// <param name="data">The data that will be encrypted</param>
    /// <param name="expectedSignature">The signature received by integration</param>
    /// <returns>If the generated signature equals to the integrations signature</returns>
    public static bool IsSignatureValid(this string expectedSignature, string secret, string data)
    {
        return GetExpectedSignature(secret, data) == expectedSignature;
    }

    /// <summary>
    /// Encrypt the <c>data</c> using <c>secret</c> as the key.
    /// </summary>
    /// <param name="secret">The client secret of integrations</param>
    /// <param name="data">The data that will be encrypted</param>
    /// <returns>A signature encrypted using the hash HmacSHA256</returns>
    private static string GetExpectedSignature(string secret, string data)
    {
        using HMACSHA256 hmacSha256 = new (Encoding.UTF8.GetBytes(secret));
        byte[] hmacBytes = hmacSha256.ComputeHash(Encoding.UTF8.GetBytes(data));
        string hmacHex = Convert.ToHexStringLower(hmacBytes);
        return hmacHex;
    }
}