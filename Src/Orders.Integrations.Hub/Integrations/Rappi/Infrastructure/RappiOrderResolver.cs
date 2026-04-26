using Orders.Integrations.Hub.Core.Application.Ports.Out.Serialization;
using Orders.Integrations.Hub.Integrations.Common.Contracts;
using Orders.Integrations.Hub.Integrations.Rappi.Domain.Entity;

namespace Orders.Integrations.Hub.Integrations.Rappi.Infrastructure;

public class RappiOrderResolver : IWebhookSignatureResolver<RappiOrder> {
    public RappiOrder Deserialize(ICustomJsonSerializer serializer, string body) 
        => serializer.Deserialize<RappiOrder>(body)!;

    public string GetMerchantId(RappiOrder request) 
        => request.Store.ExternalId;
}