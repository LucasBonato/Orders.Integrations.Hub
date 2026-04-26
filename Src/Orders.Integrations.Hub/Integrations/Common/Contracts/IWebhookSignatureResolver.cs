using Orders.Integrations.Hub.Core.Application.Ports.Out.Serialization;

namespace Orders.Integrations.Hub.Integrations.Common.Contracts;

public interface IWebhookSignatureResolver<TRequest> {
    TRequest Deserialize(ICustomJsonSerializer serializer, string body);
    string GetMerchantId(TRequest request);
}