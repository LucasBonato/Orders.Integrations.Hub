using System.Security.Cryptography;
using System.Text;

namespace BizPik.Orders.Hub.Modules.Integrations.Rappi.Validators;

public static class RappiSignatureValidator
{
    /// <summary>
    /// Compara a assinatura passada com a assinatura criada <br/>
    /// pela aplicação, para verificar se é corpo enviado e válido.
    /// </summary>
    /// <param name="secret">Client_Secret do Rappi</param>
    /// <param name="data">Body do payload recebido no contexto</param>
    /// <param name="expectedSignature">Assinatura do `Rappi-Signature`</param>
    /// <returns>Retorna se a assinatura é válida</returns>
    public static bool VerifyHmacSHA256(string secret, string data, string expectedSignature) {
        return GetExpectedSignature(secret, data) == expectedSignature;
    }

    /// <summary>
    /// Faz a encriptação dos dados utilizando a secret utilizando o padrão HmacSha-256.
    /// </summary>
    /// <param name="secret">Client_Secret da Rappi</param>
    /// <param name="data">Body do payload recebido no contexto</param>
    /// <returns>Retorna uma assinatura encriptada em HmacSha-256</returns>
    public static string GetExpectedSignature(string secret, string data) {
        using HMACSHA256 hmacSha256 = new HMACSHA256(Encoding.UTF8.GetBytes(secret));
        byte[] hmacBytes = hmacSha256.ComputeHash(Encoding.UTF8.GetBytes(data));
        string hmacHex = BitConverter.ToString(hmacBytes).Replace("-", "").ToLower();
        return hmacHex;
    }
}