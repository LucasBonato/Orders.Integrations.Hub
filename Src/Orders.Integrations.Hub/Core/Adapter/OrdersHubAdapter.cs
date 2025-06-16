using Orders.Integrations.Hub.Core.Application.Extensions;
using Orders.Integrations.Hub.Core.Domain.Contracts.UseCases;
using Orders.Integrations.Hub.Core.Domain.ValueObjects.DTOs.;
using Orders.Integrations.Hub.Core.Domain.ValueObjects.DTOs.Request;
using Orders.Integrations.Hub.Core.Domain.ValueObjects.Enums;

using Microsoft.AspNetCore.Mvc;

using static Microsoft.AspNetCore.Http.Results;

namespace Orders.Integrations.Hub.Core.Adapter;

public abstract class OrdersHubAdapterLog;

public static class OrdersHubAdapter
{
    public static async Task<IResult> GetIntegrationCancellationReason(
        ILogger<OrdersHubAdapterLog> logger,
        [FromQuery] string? externalOrderId,
        [FromQuery] OrderIntegration integration,
        [FromServices] IServiceProvider serviceProvider
    ) {
        IOrderGetCancellationReasonUseCase service = serviceProvider.GetRequiredKeyedService<IOrderGetCancellationReasonUseCase>(integration);
        return Ok(await service.ExecuteAsync(externalOrderId));
    }

    public static async Task<IResult> ChangeIntegrationStatus(
        [FromBody] ChangeOrderStatusRequest request,
        [FromServices] IServiceProvider serviceProvider
    ) {
        var useCase = serviceProvider.GetRequiredKeyedService<IOrderChangeStatusUseCase>(request.Integration);
        await useCase.ExecuteAsync(request);
        return Ok();
    }

    public static async Task<IResult> EnableIntegrationProduct(
        HttpRequest request,
        [FromServices] IServiceProvider serviceProvider
    ) {
        SNSProductEvent body = await request.ReadBodyFromSNS<SNSProductEvent>();
        var useCase = serviceProvider.GetRequiredKeyedService<IOrderChangeProductStatusUseCase>(body.Integration);
        await useCase.Enable(body);
        return Ok();
    }

    public static async Task<IResult> DisableIntegrationProduct(
        HttpRequest request,
        [FromServices] IServiceProvider serviceProvider
    ) {
        SNSProductEvent body = await request.ReadBodyFromSNS<SNSProductEvent>();
        var useCase = serviceProvider.GetRequiredKeyedService<IOrderChangeProductStatusUseCase>(body.Integration);
        await useCase.Disable(body);
        return Ok();
    }

    public static async Task<IResult> PostResponseDisputeIntegration(
        [FromBody] RespondDisputeIntegrationRequest request,
        [FromServices] IServiceProvider serviceProvider
    ) {
        var useCase = serviceProvider.GetRequiredKeyedService<IOrderDisputeRespondUseCase>(request.Integration);
        await useCase.ExecuteAsync(request);
        return Ok();
    }
}