using Orders.Integrations.Hub.Integrations.Common.Contracts;

namespace Orders.Integrations.Hub.Integrations.Common.Extensions;

public static class HttpRequestMessageExtension
{
    private static readonly HttpRequestOptionsKey<IIntegrationContext> IntegrationKey = new("IntegrationContext");

    extension(HttpRequestMessage request) {
        public void SetIntegrationContext(IIntegrationContext integrationContext) {
            request.Options.Set(IntegrationKey, integrationContext);
        }

        public IIntegrationContext GetIntegrationContext() {
            return (request.Options.TryGetValue(IntegrationKey, out var integrationContext))
                ? integrationContext
                : throw new InvalidOperationException("IntegrationContext was not set on the request");
        }
    }
}