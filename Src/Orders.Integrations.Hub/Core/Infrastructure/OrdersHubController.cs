using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

using Orders.Integrations.Hub.Core.Application.Extensions;
using Orders.Integrations.Hub.Core.Application.Integration;
using Orders.Integrations.Hub.Core.Domain.Contracts.UseCases.Integrations.Out;
using Orders.Integrations.Hub.Core.Domain.ValueObjects.DTOs.Request;
using Orders.Integrations.Hub.Core.Domain.ValueObjects.DTOs.Response;

using static Microsoft.AspNetCore.Http.TypedResults;

namespace Orders.Integrations.Hub.Core.Infrastructure;

public class OrdersHubController
{
    public static async Task<Ok<List<CancellationReasonsResponse>>> GetIntegrationCancellationReason(
        ILogger<OrdersHubController> logger,
        [FromQuery] string? externalOrderId,
        [FromQuery] IntegrationKey integration,
        [FromServices] IIntegrationRouter router
    ) {
        var useCase = router.Resolve<IOrderGetCancellationReasonUseCase>(integration);
        return Ok(await useCase.ExecuteAsync(externalOrderId));
    }

    public static async Task<NoContent> ChangeIntegrationStatus(
        [FromBody] ChangeOrderStatusRequest request,
        [FromServices] IIntegrationRouter router
    ) {
        var useCase = router.Resolve<IOrderChangeStatusUseCase>(request.Integration);
        await useCase.ExecuteAsync(request);
        return NoContent();
    }

    public static async Task<NoContent> EnableIntegrationProduct(
        HttpRequest request,
        [FromServices] IIntegrationRouter router
    ) {
        object body = await request.ReadBodyFromSNS<object>();
        var useCase = router.Resolve<IOrderChangeProductStatusUseCase>(IntegrationKey.Nothing());
        await useCase.Enable(body);
        return NoContent();
    }

    public static async Task<NoContent> DisableIntegrationProduct(
        HttpRequest request,
        [FromServices] IIntegrationRouter router
    ) {
        object body = await request.ReadBodyFromSNS<object>();
        var useCase = router.Resolve<IOrderChangeProductStatusUseCase>(IntegrationKey.Nothing());
        await useCase.Disable(body);
        return NoContent();
    }

    public static async Task<NoContent> PostResponseDisputeIntegration(
        [FromBody] RespondDisputeIntegrationRequest request,
        [FromServices] IIntegrationRouter router
    ) {
        var useCase = router.Resolve<IOrderDisputeRespondUseCase>(request.Integration);
        await useCase.ExecuteAsync(request);
        return NoContent();
    }
}