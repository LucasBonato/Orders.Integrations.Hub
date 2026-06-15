namespace Orders.Integrations.Hub.Integrations.Common.Contracts;

public interface ISignatureValidator
{
    /// <summary>
    /// Validates a given <c>signature</c> by generating a new signature using the provided <c>body</c>
    /// and <c>secret</c>.
    /// </summary>
    /// <param name="signature">The signature compared.</param>
    /// <param name="rawBody">The raw body data of the request.</param>
    /// <param name="secret">The secret used to generate the signature.</param>
    /// <returns>
    /// <c>true</c> if the generated signature matches the provided <c>signature</c>;
    /// otherwise, <c>false</c>.
    /// </returns>
    bool IsValid(string signature, byte[] rawBody, string secret);
    
    /// <summary>
    /// Validates a given <c>signature</c> by generating a new signature using the provided <c>body</c>
    /// and <c>secret</c>.
    /// </summary>
    /// <param name="signature">The signature compared.</param>
    /// <param name="body">The raw body data of the request.</param>
    /// <param name="secret">The secret used to generate the signature.</param>
    /// <returns>
    /// <c>true</c> if the generated signature matches the provided <c>signature</c>;
    /// otherwise, <c>false</c>.
    /// </returns>
    bool IsValid(string signature, string body, string secret);
}