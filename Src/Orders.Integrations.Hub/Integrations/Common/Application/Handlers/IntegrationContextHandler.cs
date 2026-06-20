using Orders.Integrations.Hub.Integrations.Common.Contracts;
using Orders.Integrations.Hub.Integrations.Common.Extensions;

namespace Orders.Integrations.Hub.Integrations.Common.Application.Handlers;

public class IntegrationContextHandler(
    IHttpContextAccessor httpContextAccessor
) : DelegatingHandler {
    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken
    ) {
        IIntegrationContext? context = httpContextAccessor.HttpContext?.RequestServices
            .GetRequiredService<IIntegrationContext>();

        if (context is not null)
            request.SetIntegrationContext(context);

        return await base.SendAsync(request, cancellationToken);
    }
}
