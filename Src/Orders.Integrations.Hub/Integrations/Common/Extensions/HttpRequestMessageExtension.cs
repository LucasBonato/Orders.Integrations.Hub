using Orders.Integrations.Hub.Integrations.Common.Contracts;

namespace Orders.Integrations.Hub.Integrations.Common.Extensions;

public static class HttpRequestMessageExtension
{
    private static readonly HttpRequestOptionsKey<IIntegrationContext> IntegrationKey = new("IntegrationContext");

    public static void SetIntegrationContext(this HttpRequestMessage request, IIntegrationContext integrationContext) {
        request.Options.Set(IntegrationKey, integrationContext);
    }

    public static IIntegrationContext GetIntegrationContext(this HttpRequestMessage request) {
        if (request.Options.TryGetValue(IntegrationKey, out var integrationContext))
            return integrationContext;

        throw new InvalidOperationException("IntegrationContext was not set on the request");
    }
}