namespace Orders.Integrations.Hub.Core.Application.Ports.In.Http;

public interface IEndpoint
{
    void MapEndpoint(IEndpointRouteBuilder app);
}