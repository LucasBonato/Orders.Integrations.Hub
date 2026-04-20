using Orders.Integrations.Hub.Core.Application.DTOs.Internal;
using Orders.Integrations.Hub.Core.Application.Ports.Out.Serialization;
using Orders.Integrations.Hub.Integrations.Common.Contracts;
using Orders.Integrations.Hub.Integrations.Common.Extensions;
using Orders.Integrations.Hub.Integrations.Common.Validators;
using Orders.Integrations.Hub.Integrations.Common.ValueObjects;
using Orders.Integrations.Hub.Integrations.IFood.Domain.ValueObjects.DTOs.Request;

namespace Orders.Integrations.Hub.Integrations.IFood.Infrastructure;

public class IFoodSignatureStrategy : IWebhookSignatureValidator, IWebhookSignatureResolver<IFoodWebhookRequest>
{
    public string HeaderName => "X-IFood-Signature";
    public string IntegrationKey => IFoodIntegrationKey.IFOOD;

    public IFoodWebhookRequest Deserialize(
        [FromKeyedServices(IFoodIntegrationKey.Value)] ICustomJsonSerializer serializer, 
        string body
    ) => serializer.Deserialize<IFoodWebhookRequest>(body)!;

    public string GetMerchantId(IFoodWebhookRequest request) 
        => request.MerchantId;
    
    public bool ValidateSignature(string signature, string body, string secret) 
        => signature.IsSignatureValid(secret, body);

    public Integration ResolveIntegration(IntegrationResponse integration) 
        => integration.ResolveIFood();
}