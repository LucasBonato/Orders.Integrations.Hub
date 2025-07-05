using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

using Orders.Integrations.Hub.Core.Application.Extensions;
using Orders.Integrations.Hub.Core.Domain.Contracts.UseCases;
using Orders.Integrations.Hub.Core.Domain.ValueObjects.DTOs.Request;
using Orders.Integrations.Hub.Core.Domain.ValueObjects.DTOs.Response;
using Orders.Integrations.Hub.Core.Domain.ValueObjects.Enums;

using static Microsoft.AspNetCore.Http.TypedResults;

namespace Orders.Integrations.Hub.Core.Adapter;

public abstract class OrdersHubAdapterLog;

public static class OrdersHubAdapter
{
    public static async Task<Ok<List<CancellationReasonsResponse>>> GetIntegrationCancellationReason(
        ILogger<OrdersHubAdapterLog> logger,
        [FromQuery] string? externalOrderId,
        [FromQuery] OrderIntegration integration,
        [FromServices] IServiceProvider serviceProvider
    ) {
        IOrderGetCancellationReasonUseCase service = serviceProvider.GetRequiredKeyedService<IOrderGetCancellationReasonUseCase>(integration);
        return Ok(await service.ExecuteAsync(externalOrderId));
    }

    public static async Task<NoContent> ChangeIntegrationStatus(
        [FromBody] ChangeOrderStatusRequest request,
        [FromServices] IServiceProvider serviceProvider
    ) {
        var useCase = serviceProvider.GetRequiredKeyedService<IOrderChangeStatusUseCase>(request.Integration);
        await useCase.ExecuteAsync(request);
        return NoContent();
    }

    public static async Task<NoContent> EnableIntegrationProduct(
        HttpRequest request,
        [FromServices] IServiceProvider serviceProvider
    ) {
        object body = await request.ReadBodyFromSNS<object>();
        var useCase = serviceProvider.GetRequiredKeyedService<IOrderChangeProductStatusUseCase>(OrderIntegration.NOTHING);
        await useCase.Enable(body);
        return NoContent();
    }

    public static async Task<NoContent> DisableIntegrationProduct(
        HttpRequest request,
        [FromServices] IServiceProvider serviceProvider
    ) {
        object body = await request.ReadBodyFromSNS<object>();
        var useCase = serviceProvider.GetRequiredKeyedService<IOrderChangeProductStatusUseCase>(OrderIntegration.NOTHING);
        await useCase.Disable(body);
        return NoContent();
    }

    public static async Task<NoContent> PostResponseDisputeIntegration(
        [FromBody] RespondDisputeIntegrationRequest request,
        [FromServices] IServiceProvider serviceProvider
    ) {
        var useCase = serviceProvider.GetRequiredKeyedService<IOrderDisputeRespondUseCase>(request.Integration);
        await useCase.ExecuteAsync(request);
        return NoContent();
    }
}