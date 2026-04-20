using Microsoft.AspNetCore.Mvc;

using Orders.Integrations.Hub.Core.Application.DTOs.Request;
using Orders.Integrations.Hub.Core.Application.Ports.In.Http;
using Orders.Integrations.Hub.Core.Application.Ports.In.Integration;
using Orders.Integrations.Hub.Core.Application.Ports.Out.UseCases;

using static Microsoft.AspNetCore.Http.Results;

namespace Orders.Integrations.Hub.Core.Adapters.In.Http;

internal sealed class OrdersHubOrderDisputeEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/Orders/Hub/Dispute", async (
            [FromServices] IIntegrationRouter router,
            [FromBody] RespondDisputeIntegrationRequest request
        ) => {
            IOrderDisputeRespondUseCase useCase = router.Resolve<IOrderDisputeRespondUseCase>(request.Integration);
            await useCase.ExecuteAsync(request);
            return NoContent();
        })
        .WithTags("Change Order Dispute Status")
        .WithDescription("Change order dispute in integrations");
    }
}