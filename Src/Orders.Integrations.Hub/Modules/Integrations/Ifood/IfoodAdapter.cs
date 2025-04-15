using Orders.Integrations.Hub.Modules.Integrations.Common.Application;
using Orders.Integrations.Hub.Modules.Integrations.Ifood.Domain.ValueObjects;
using Orders.Integrations.Hub.Modules.Integrations.Ifood.Domain.ValueObjects.Enums;

using Microsoft.AspNetCore.Mvc;
using static Microsoft.AspNetCore.Http.Results;

namespace Orders.Integrations.Hub.Modules.Integrations.Ifood;

public abstract class IfoodAdapterLog;

public static class IfoodAdapter
{
    public static async Task<IResult> Webhook(
        ILogger<IfoodAdapterLog> logger,
        [FromServices] ICreateOrderUseCase<IfoodWebhookRequest> createOrder,
        [FromServices] IUpdateOrderStatusUseCase<IfoodWebhookRequest> updateOrder,
        [FromBody] IfoodWebhookRequest request
    ) {
        logger.LogInformation("[INFO] - IfoodAdapter - Ifood Webhook code: {FullCode}", request.FullCode);

        return request.FullCode switch
        {
            IfoodFullOrderStatus.KEEPALIVE => Accepted(),

            IfoodFullOrderStatus.PLACED => Accepted("/", await createOrder.ExecuteAsync(request)),

            IfoodFullOrderStatus.CONFIRMED or
            IfoodFullOrderStatus.SEPARATION_STARTED or
            IfoodFullOrderStatus.SEPARATION_ENDED or
            IfoodFullOrderStatus.READY_TO_PICKUP or
            IfoodFullOrderStatus.DISPATCHED or
            IfoodFullOrderStatus.CONCLUDED or
            IfoodFullOrderStatus.CANCELLED => Accepted( "/", await updateOrder.ExecuteAsync(request)),

            _ => BadRequest(new { error = $"not mapped but ok {request.FullCode}" })
        };
    }
}

// return request.FullCode switch
// {
//     IfoodFullOrderStatus.KEEPALIVE => Accepted(),
//
//     IfoodFullOrderStatus.PLACED => Accepted("/",
//         await ifoodTo.OrderTo(
//             new(
//                 MerchantId: request.MerchantId!,
//                 OrderId: request.OrderId!,
//                 CreatedAt: request.CreatedAt ?? DateTime.UtcNow
//             )
//         )
//     ),
//
//     IfoodFullOrderStatus.CONFIRMED or
//         IfoodFullOrderStatus.SEPARATION_STARTED or
//         IfoodFullOrderStatus.SEPARATION_ENDED or
//         IfoodFullOrderStatus.READY_TO_PICKUP or
//         IfoodFullOrderStatus.DISPATCHED or
//         IfoodFullOrderStatus.CONCLUDED or
//         IfoodFullOrderStatus.CANCELLED
//         => Accepted( "/",
//             await ifoodTo.ChangeOrderStatus(
//                 new IfoodOrderChangeOrderStatusInput(
//                     MerchantId: request.MerchantId!,
//                     OrderId: request.OrderId!,
//                     Status: IfoodOrderStatusTypesExtensions.FromCode(request.FullCode),
//                     CreatedAt: request.CreatedAt ?? DateTime.UtcNow
//                 )
//             )
//         ),
//
//     _ => Accepted("/", new { message = $"not mapped but ok {request.FullCode}" })
// };