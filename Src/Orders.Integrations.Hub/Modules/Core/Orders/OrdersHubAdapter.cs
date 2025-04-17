using Orders.Integrations.Hub.Modules.Core.Orders.Domain.ValueObjects.DTOs.Request;
using Orders.Integrations.Hub.Modules.Core.Orders.Domain.ValueObjects.Enums;
using Orders.Integrations.Hub.Modules.Integrations.Ifood.Application.Ports;

using Microsoft.AspNetCore.Mvc;

using static Microsoft.AspNetCore.Http.Results;

namespace Orders.Integrations.Hub.Modules.Core.Orders;

public static class OrdersHubAdapter
{
    public static async Task<IResult> ChangeIntegrationStatus(
        [FromServices] IfoodChangeOrderStatusUseCase ifoodUseCase,
        [FromBody] ChangeOrderStatusRequest request
    ) {
        switch (request.Integration) {
            case OrderIntegration.IFOOD:
                await ifoodUseCase.Execute(request);
                break;
            // case OrderIntegration.RAPPI:
            //     await rappiUseCase.Execute(request);
            //     break;
            default:
                return BadRequest("Invalid integration");
        }
        return Ok();
    }
}