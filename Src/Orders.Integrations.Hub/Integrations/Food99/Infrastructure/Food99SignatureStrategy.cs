using Orders.Integrations.Hub.Core.Application.DTOs.Internal;
using Orders.Integrations.Hub.Core.Application.Ports.Out.Serialization;
using Orders.Integrations.Hub.Integrations.Common.Contracts;
using Orders.Integrations.Hub.Integrations.Common.Extensions;
using Orders.Integrations.Hub.Integrations.Common.Validators;
using Orders.Integrations.Hub.Integrations.Common.ValueObjects;
using Orders.Integrations.Hub.Integrations.Food99.Application.ValueObjects;
using Orders.Integrations.Hub.Integrations.Food99.Domain.Entity;

namespace Orders.Integrations.Hub.Integrations.Food99.Infrastructure;

public class Food99SignatureStrategy(
    Md5SignatureValidator signatureValidator
) : IWebhookSignatureValidator, IWebhookSignatureResolver<Food99WebhookRequest> {
    public string HeaderName => "didi-header-sign";
    public string IntegrationKey => Food99IntegrationKey.FOOD99;

    public Food99WebhookRequest Deserialize(
        [FromKeyedServices(Food99IntegrationKey.Value)] ICustomJsonSerializer serializer, 
        string body
    ) => serializer.Deserialize<Food99WebhookRequest>(body)!;

    public string GetMerchantId(Food99WebhookRequest request) 
        => request.AppShopId;
    
    public bool ValidateSignature(string signature, string body, string secret) 
        => signatureValidator.IsValid(signature, body, secret);

    public Integration ResolveIntegration(IntegrationResponse integration) 
        => integration.Resolve99Food();
}