using System.Text;
using Orders.Integrations.Hub.Core.Application.DTOs.Internal;
using Orders.Integrations.Hub.Core.Application.Ports.Out.Clients;
using Orders.Integrations.Hub.Core.Application.Ports.Out.Serialization;
using Orders.Integrations.Hub.Integrations.Common.Contracts;

namespace Orders.Integrations.Hub.Integrations.Common.Middleware;

public class WebhookSignatureFilter<TRequest, TValidator, TResolver>(
    TValidator validator,
    TResolver resolver,
    ILogger<WebhookSignatureFilter<TRequest, TValidator, TResolver>> logger
) : IEndpointFilter 
    where TValidator : IWebhookSignatureValidator
    where TResolver : IWebhookSignatureResolver<TRequest> {
    
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next) {
        HttpContext httpContext = context.HttpContext;
        IServiceProvider services = httpContext.RequestServices;

        ICustomJsonSerializer serializer = services.GetRequiredKeyedService<ICustomJsonSerializer>(validator.IntegrationKey);
        IInternalClient internalClient = services.GetRequiredService<IInternalClient>();
        IIntegrationContext integrationContext = services.GetRequiredService<IIntegrationContext>();

        using StreamReader reader = new(httpContext.Request.Body, Encoding.UTF8, detectEncodingFromByteOrderMarks: false, leaveOpen: true);
        string body = await reader.ReadToEndAsync();
        
        if (logger.IsEnabled(LogLevel.Information))
            logger.LogInformation("[INFO] - {Validator} - Request body: {body}", typeof(TValidator).Name, body);
        
        string? signature = httpContext.Request.Headers[validator.HeaderName].FirstOrDefault();

        if (signature is null) {
            logger.LogWarning("[WARN] - {Validator} - Signature Header '{Header}' is missing", typeof(TValidator).Name, validator.HeaderName);
            return Results.Problem("Signature header is missing.", statusCode: StatusCodes.Status401Unauthorized);
        }
        
        TRequest request = resolver.Deserialize(serializer, body)!;
        httpContext.Items["WebhookRequest"] = request;

        string merchantId = resolver.GetMerchantId(request);
        IntegrationResponse rawIntegration = await internalClient.GetIntegrationByExternalId(merchantId);
        integrationContext.MerchantId = merchantId;
        integrationContext.Integration = validator.ResolveIntegration(rawIntegration);

        string payload = validator.PrepareSignaturePayload(signature, body);
        string secret = integrationContext.Integration.ClientSecret;
        
        if (!validator.ValidateSignature(signature, payload, secret)) {
            logger.LogWarning("[WARN] - {Validator} - Invalid signature.", typeof(TValidator).Name);
            return Results.Problem("Invalid Signature", statusCode: StatusCodes.Status401Unauthorized);
        }
        
        return next(context);
    }
}