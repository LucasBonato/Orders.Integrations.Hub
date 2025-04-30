using FastEndpoints;

namespace BizPik.Orders.Hub.Modules.Core.Orders;

public class OrdersHubHealthEndpoint : Endpoint<EmptyRequest, string>
{
    public override void Configure()
    {
        Get("/health");
        AllowAnonymous();
    }

    public override Task<string> ExecuteAsync(EmptyRequest req, CancellationToken ct)
    {
        return Task.Run(() => "Healthy");
    }
}