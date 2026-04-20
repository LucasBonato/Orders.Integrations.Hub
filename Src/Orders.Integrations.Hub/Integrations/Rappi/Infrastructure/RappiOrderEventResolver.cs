using Orders.Integrations.Hub.Core.Application.Ports.Out.Serialization;
using Orders.Integrations.Hub.Integrations.Common.Contracts;
using Orders.Integrations.Hub.Integrations.Rappi.Domain.ValueObjects.DTOs.Request;

namespace Orders.Integrations.Hub.Integrations.Rappi.Infrastructure;

public class RappiOrderEventResolver : IWebhookSignatureResolver<RappiWebhookEventOrderRequest>
{
    public RappiWebhookEventOrderRequest Deserialize(ICustomJsonSerializer serializer, string body)
        => serializer.Deserialize<RappiWebhookEventOrderRequest>(body)!;

    public string GetMerchantId(RappiWebhookEventOrderRequest request)
        => request.StoreId;
}