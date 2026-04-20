using Microsoft.AspNetCore.Mvc;

using Orders.Integrations.Hub.Core.Application.DTOs.Response;
using Orders.Integrations.Hub.Core.Application.Ports.In.Http;
using Orders.Integrations.Hub.Core.Application.Ports.In.Integration;
using Orders.Integrations.Hub.Core.Application.Ports.Out.UseCases;
using Orders.Integrations.Hub.Core.Domain.ValueObjects;

using static Microsoft.AspNetCore.Http.Results;

namespace Orders.Integrations.Hub.Core.Adapters.In.Http;

internal sealed class OrdersHubOrderCancellationReasonEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("Orders/Hub/Cancellation-Reason", async (
            [FromServices] IIntegrationRouter router,
            [FromQuery] string? externalOrderId,
            [FromQuery] string integration
        ) => {
            var useCase = router.Resolve<IOrderGetCancellationReasonUseCase>(IntegrationKey.From(integration));
            return Ok(await useCase.ExecuteAsync(externalOrderId));
        })
        .WithTags("Cancellation Reason")
        .WithDescription("Get cancellation reason of the order by integration")
        .Produces<List<CancellationReasonsResponse>>()
        .ProducesValidationProblem()
        .CacheOutput();
    }
}