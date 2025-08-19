using System.Security.Cryptography;
using System.Text;

namespace Orders.Integrations.Hub.Integrations.Common.Validators;

public static class SignatureMd5Validator
{
    /// <summary>
    /// Validates a given MD5 <c>signature</c> by generating a new signature using the provided <c>body</c>
    /// and <c>secret</c>.
    /// </summary>
    /// <param name="signature">The signature received from the integration to be validated.</param>
    /// <param name="body">The raw body data of the request.</param>
    /// <param name="secret">The client secret used to generate the signature.</param>
    /// <returns>
    /// <c>true</c> if the generated MD5 signature matches the provided <c>signature</c>;
    /// otherwise, <c>false</c>.
    /// </returns>
    /// <remarks>
    /// This method concatenates <paramref name="body"/> and <paramref name="secret"/>,
    /// generates the MD5 hash, and compares it to <paramref name="signature"/> in a
    /// case-insensitive manner.
    /// </remarks>
    public static bool IsValidateMd5Signature(this string signature, string body, string secret)
    {
        string signedBody = $"{body}{secret}";

        byte[] inputBytes = Encoding.UTF8.GetBytes(signedBody);
        byte[] hashBytes = MD5.HashData(inputBytes);

        StringBuilder sb = new();
        foreach (byte b in hashBytes)
            sb.Append(b.ToString("x2"));

        string calculatedSignature = sb.ToString();
        return string.Equals(calculatedSignature, signature, StringComparison.OrdinalIgnoreCase);
    }
}