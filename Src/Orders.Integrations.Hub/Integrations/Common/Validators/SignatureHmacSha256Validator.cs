using System.Security.Cryptography;
using System.Text;

namespace Orders.Integrations.Hub.Integrations.Common.Validators;

public static class SignatureHmacSha256Validator
{
    public static bool IsSignatureValid(this string expectedSignature, string secret, byte[] rawBody)
    {
        byte[] computedHash = GetExpectedSignatureBytes(secret, rawBody);
        byte[] expectedBytes = Convert.FromHexString(expectedSignature);

        // Constant-time comparison — prevents timing attacks as the docs recommend
        return CryptographicOperations.FixedTimeEquals(computedHash, expectedBytes);
    }
    
    /// <summary>
    /// Compares the <c>expectedSignature</c> to a generated signature using a secret and some data.
    /// </summary>
    /// <param name="secret">The client secret of integrations</param>
    /// <param name="data">The data that will be encrypted</param>
    /// <param name="expectedSignature">The signature received by integration</param>
    /// <returns>If the generated signature equals to the integrations signature</returns>
    public static bool IsSignatureValid(this string expectedSignature, string secret, string data)
    {
        byte[] rawBody = Encoding.UTF8.GetBytes(data);
        return expectedSignature.IsSignatureValid(secret, rawBody);
    }

    /// <summary>
    /// Encrypt the <c>data</c> using <c>secret</c> as the key.
    /// </summary>
    /// <param name="secret">The client secret of integrations</param>
    /// <param name="data">The data that will be encrypted</param>
    /// <returns>A signature encrypted using the hash HmacSHA256</returns>
    private static byte[] GetExpectedSignatureBytes(string secret, byte[] data) {
        using HMACSHA256 hmacSha256 = new(Encoding.UTF8.GetBytes(secret));
        return hmacSha256.ComputeHash(data);
    }
}