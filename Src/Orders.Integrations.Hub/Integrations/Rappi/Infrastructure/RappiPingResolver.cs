using Orders.Integrations.Hub.Core.Application.Ports.Out.Serialization;
using Orders.Integrations.Hub.Integrations.Common.Contracts;
using Orders.Integrations.Hub.Integrations.Rappi.Domain.ValueObjects.DTOs.Request;

namespace Orders.Integrations.Hub.Integrations.Rappi.Infrastructure;

public class RappiPingResolver : IWebhookSignatureResolver<RappiWebhookPingRequest>
{
    public RappiWebhookPingRequest Deserialize(ICustomJsonSerializer serializer, string body)
        => serializer.Deserialize<RappiWebhookPingRequest>(body)!;

    public string GetMerchantId(RappiWebhookPingRequest request)
        => request.StoreId.ToString();
}