using System.Security.Cryptography;
using System.Security.Cryptography.Xml;
using System.Text;

using FastEndpoints;

using Microsoft.AspNetCore.Http.HttpResults;

namespace BizPik.Orders.Hub.Modules.Integrations.Saleschannels.Adapters.Ifood.Validators;

public class IfoodSignatureValidator<TRequest> : IPreProcessor<TRequest>
{
    public async Task PreProcessAsync(IPreProcessorContext<TRequest> context, CancellationToken ct)
    {
        string? body;
        using (StreamReader reader = new(context.HttpContext.Request.Body, Encoding.UTF8))
        {
            body = await reader.ReadToEndAsync();
        }

        if (IsSignatureValid(signature, body)) return BadRequest();

        throw new NotImplementedException();
    }

    private static bool IsSignatureValid(string headerSignature, string body, ILogger<IfoodSignatureValidator<TRequest>> logger)
    {
        logger.LogInformation("[INFO] Header signature: {HeaderSignature}", body);

        string generatedSignature = GetExpectedSignature(AppEnv.IFOOD.CLIENT.SECRET.NotNull(), body);

        logger.LogInformation("[INFO] [ Generated signature: {GeneratedSignature}\nExpected signature: {ExpectedSignature} ]", generatedSignature, headerSignature);

        return generatedSignature == headerSignature;
    }

    private static string GetExpectedSignature(string secret, string data)
    {
        using HMACSHA256 hmacSha256 = new (Encoding.UTF8.GetBytes(secret));
        byte[] hmacBytes = hmacSha256.ComputeHash(Encoding.UTF8.GetBytes(data));
        string hmacHex = BitConverter.ToString(hmacBytes).Replace("-", "").ToLower();
        return hmacHex;
    }
}