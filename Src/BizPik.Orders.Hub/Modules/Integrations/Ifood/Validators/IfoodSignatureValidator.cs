using System.Text;
using System.Security.Cryptography;
using static Microsoft.AspNetCore.Http.Results;

namespace BizPik.Orders.Hub.Modules.Integrations.Ifood.Validators;

public class IfoodSignatureValidator(
    ILogger<IfoodSignatureValidator> logger
) : IEndpointFilter {

    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        HttpContext httpContext = context.HttpContext;

        httpContext.Request.EnableBuffering();
        httpContext.Request.Body.Position = 0;
        logger.LogInformation("IfoodSignatureValidator invoked: {ahsdua}", httpContext.Request.Body.Length);

        string body;
        using (var reader = new StreamReader(httpContext.Request.Body, Encoding.UTF8, detectEncodingFromByteOrderMarks: false, leaveOpen: true))
        {
            body = await reader.ReadToEndAsync();
            httpContext.Request.Body.Position = 0;
        }

        logger.LogInformation("[INFO] - IfoodSignatureValidator - Request Body: {body}", body);

        string? signature = httpContext.Request.Headers["X-Ifood-Signature"].FirstOrDefault();

        logger.LogInformation("[INFO] - IfoodSignatureValidator - X-Ifood-Signature: {signature}", signature);

        if (signature == null)
        {
            return BadRequest("Signature header is missing.");
        }

        if (!IsSignatureValid(signature, body))
        {
            return BadRequest("Invalid signature");
        }

        return await next(context);
    }

    private bool IsSignatureValid(string headerSignature, string body)
    {
        logger.LogInformation("[INFO] - IfoodSignatureValidator - Expected Signature: [{signature}]", headerSignature);

        string generatedSignature = GetExpectedSignature(AppEnv.INTEGRATIONS.IFOOD.CLIENT.SECRET.NotNull(), body);

        logger.LogInformation("[INFO] - IfoodSignatureValidator - Generated Signature: [{signature}]", generatedSignature);

        return generatedSignature == headerSignature;
    }

    private static string GetExpectedSignature(string secret, string data)
    {
        using HMACSHA256 hmacSha256 = new (Encoding.UTF8.GetBytes(secret));
        byte[] hmacBytes = hmacSha256.ComputeHash(Encoding.UTF8.GetBytes(data));
        string hmacHex = Convert.ToHexStringLower(hmacBytes);
        return hmacHex;
    }
}