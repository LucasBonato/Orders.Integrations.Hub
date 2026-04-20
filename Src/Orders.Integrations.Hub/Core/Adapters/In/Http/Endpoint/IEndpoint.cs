namespace Orders.Integrations.Hub.Core.Adapters.In.Http.Endpoint;

public interface IEndpoint
{
    void MapEndpoint(IEndpointRouteBuilder app);
}