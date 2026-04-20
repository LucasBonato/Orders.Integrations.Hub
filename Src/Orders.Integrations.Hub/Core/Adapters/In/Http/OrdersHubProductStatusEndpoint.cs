using Microsoft.AspNetCore.Mvc;

using Orders.Integrations.Hub.Core.Adapters.In.Http.Endpoint;
using Orders.Integrations.Hub.Core.Application.Ports.In.Integration;
using Orders.Integrations.Hub.Core.Application.Ports.Out.UseCases;
using Orders.Integrations.Hub.Core.Domain.ValueObjects;
using Orders.Integrations.Hub.Core.Infrastructure.Extensions;

using static Microsoft.AspNetCore.Http.Results;

namespace Orders.Integrations.Hub.Core.Adapters.In.Http;

internal sealed class OrdersHubProductStatusEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        RouteGroupBuilder group = app
                .MapGroup("/Orders/Hub/Product")
                .WithTags("Product Status")
                .WithDescription("Change status of the products in integrations")
            ;
        
        group.MapPost("/Enable", async (
            [FromServices] IIntegrationRouter router,
            HttpRequest request
        ) => {
            object body = await request.ReadBodyFromSNS<object>();
            await router.Resolve<IOrderChangeProductStatusUseCase>(IntegrationKey.Nothing()).Enable(body);
            return NoContent();
        });
        
        group.MapPost("/Disable", async (
            [FromServices] IIntegrationRouter router,
            HttpRequest request
        ) => {
            object body = await request.ReadBodyFromSNS<object>();
            await router.Resolve<IOrderChangeProductStatusUseCase>(IntegrationKey.Nothing()).Disable(body);
            return NoContent();
        });
    }
}