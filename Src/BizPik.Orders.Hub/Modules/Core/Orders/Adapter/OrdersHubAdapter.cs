using BizPik.Orders.Hub.Modules.Core.BizPik.Domain.ValueObjects;
using BizPik.Orders.Hub.Modules.Core.Orders.Application.Extensions;
using BizPik.Orders.Hub.Modules.Core.Orders.Domain.Contracts.Providers;
using BizPik.Orders.Hub.Modules.Core.Orders.Domain.Contracts.UseCases;
using BizPik.Orders.Hub.Modules.Core.Orders.Domain.ValueObjects.DTOs.Request;
using BizPik.Orders.Hub.Modules.Core.Orders.Domain.ValueObjects.Enums;

using Microsoft.AspNetCore.Mvc;

using static Microsoft.AspNetCore.Http.Results;

namespace BizPik.Orders.Hub.Modules.Core.Orders.Adapter;

public abstract class OrdersHubAdapterLog;

public static class OrdersHubAdapter
{
    public static async Task<IResult> GetIntegrationCancellationReason(
        ILogger<OrdersHubAdapterLog> logger,
        [FromQuery] string? orderId,
        [FromQuery] OrderIntegration integration,
        [FromServices] IServiceProvider serviceProvider
    ) {
        IOrderGetCancellationReasonUseCase service = serviceProvider.GetRequiredKeyedService<IOrderGetCancellationReasonUseCase>(integration);
        return Ok(await service.ExecuteAsync(orderId));
    }

    public static async Task<IResult> ChangeIntegrationStatus(
        [FromBody] ChangeOrderStatusRequest request,
        [FromServices] IOrderChangeStatusUseCaseProvider provider
    ) {
        var useCase = provider.Get(request.Integration);
        await useCase.Execute(request);

        return Ok();
    }

    public static async Task EnableIntegrationProduct(
        HttpRequest request,
        [FromServices] IOrderChangeProductStatusUseCaseProvider provider
    ) {
        BizPikSNSProductEvent body = await request.ReadBodyFromSNS<BizPikSNSProductEvent>();
        var useCase = provider.Get(body.Integration);
        await useCase.Enable(body);
    }

    public static async Task DisableIntegrationProduct(
        HttpRequest request,
        [FromServices] IOrderChangeProductStatusUseCaseProvider provider
    ) {
        BizPikSNSProductEvent body = await request.ReadBodyFromSNS<BizPikSNSProductEvent>();
        var useCase = provider.Get(body.Integration);
        await useCase.Disable(body);
    }
}