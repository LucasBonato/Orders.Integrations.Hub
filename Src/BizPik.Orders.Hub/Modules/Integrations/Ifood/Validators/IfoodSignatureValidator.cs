using System.Text;

using BizPik.Orders.Hub.Modules.Integrations.Common.Validators;

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

        if (!signature.IsSignatureValid(signature, body))
        {
            return BadRequest("Invalid signature");
        }

        return await next(context);
    }
}