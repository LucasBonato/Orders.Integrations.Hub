using Microsoft.AspNetCore.Mvc;

using Orders.Integrations.Hub.Core.Adapters.In.Http.Endpoint;
using Orders.Integrations.Hub.Core.Application.DTOs.Request;
using Orders.Integrations.Hub.Core.Application.Ports.In.Integration;
using Orders.Integrations.Hub.Core.Application.Ports.Out.UseCases;

using static Microsoft.AspNetCore.Http.Results;

namespace Orders.Integrations.Hub.Core.Adapters.In.Http;

internal sealed class OrdersHubOrderStatusEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPatch("/Orders/Hub/Status", async (
            [FromServices] IIntegrationRouter router,
            [FromBody] ChangeOrderStatusRequest request
        ) => {
            IOrderChangeStatusUseCase useCase = router.Resolve<IOrderChangeStatusUseCase>(request.Integration);
            await useCase.ExecuteAsync(request);
            return NoContent();
        })
        .WithTags("Change Order Status")
        .WithDescription("Change order status in integrations")
        .ProducesValidationProblem();
    }
}