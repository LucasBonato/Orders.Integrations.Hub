using FastEndpoints;

namespace Orders.Integrations.Hub.Core.Infrastructure;

public class OrdersHubHealthEndpoint : Endpoint<EmptyRequest, string>
{
    public override void Configure()
    {
        Get("/health");
        AllowAnonymous();
    }

    public override Task<string> ExecuteAsync(EmptyRequest req, CancellationToken ct)
    {
        return Task.FromResult("Healthy");
    }
}