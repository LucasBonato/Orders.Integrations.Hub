using BizPik.Orders.Hub.Modules.Core.BizPik.Domain.ValueObjects;
using BizPik.Orders.Hub.Modules.Core.Orders.Application.Extensions;
using BizPik.Orders.Hub.Modules.Core.Orders.Domain.Contracts.Providers;
using BizPik.Orders.Hub.Modules.Core.Orders.Domain.ValueObjects.DTOs.Request;

using Microsoft.AspNetCore.Mvc;

using static Microsoft.AspNetCore.Http.Results;

namespace BizPik.Orders.Hub.Modules.Core.Orders.Adapter;

public static class OrdersHubAdapter
{
    public static Task GetIntegrationCancellationReason(
        HttpContext context
    ) {
        throw new NotImplementedException();
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