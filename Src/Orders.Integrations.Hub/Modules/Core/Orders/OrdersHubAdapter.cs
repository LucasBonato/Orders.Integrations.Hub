using Orders.Integrations.Hub.Modules.Core..Domain.ValueObjects;
using Orders.Integrations.Hub.Modules.Core.Orders.Application.Extensions;
using Orders.Integrations.Hub.Modules.Core.Orders.Domain.Contracts.Providers;
using Orders.Integrations.Hub.Modules.Core.Orders.Domain.ValueObjects.DTOs.Request;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.AspNetCore.Http.Results;

namespace Orders.Integrations.Hub.Modules.Core.Orders;

public static class OrdersHubAdapter
{
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
        SNSProductEvent body = await request.ReadBodyFromSNS<SNSProductEvent>();
        var useCase = provider.Get(body.Integration);
        await useCase.Enable(body);
    }

    public static async Task DisableIntegrationProduct(
        HttpRequest request,
        [FromServices] IOrderChangeProductStatusUseCaseProvider provider
    ) {
        SNSProductEvent body = await request.ReadBodyFromSNS<SNSProductEvent>();
        var useCase = provider.Get(body.Integration);
        await useCase.Disable(body);
    }
}