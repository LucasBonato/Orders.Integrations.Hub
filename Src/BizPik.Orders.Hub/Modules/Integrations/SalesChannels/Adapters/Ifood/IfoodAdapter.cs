using System.Text;
using System.Text.Json;

using BizPik.Orders.Hub.Modules.Integrations.Saleschannels.Adapters.Ifood.Dtos;

using Microsoft.AspNetCore.Mvc;

namespace BizPik.Orders.Hub.Modules.Integrations.Saleschannels.Adapters.Ifood;

public abstract class IfoodAdapterLog;

public static class IfoodAdapter
{
    public static async Task Webhook(
        HttpContext context,
        ILogger<IfoodAdapterLog> logger,
        [FromHeader(Name = "X-Ifood-Signature")] string signature,
        [FromServices] IfoodToBizPikService ifoodToBizPik
    ) {
        string? strBody;

            using (StreamReader reader = new(context.Request.Body, Encoding.UTF8)) {
                strBody = await reader.ReadToEndAsync();
            }

            logger.LogInformation("[INFO] Request body: {strBody}", strBody);

            IfoodRequest? request = JsonSerializer.Deserialize<IfoodRequest>(strBody);

            if (IsSignatureValid(Signature, strBody, logger) is false)
            {
                return BadRequest(new { Reason = "Invalid signature" });
            }

            logger.LogInformation("[INFO] Ifood webhook code: {FullCode}", request?.Fullcode);

            IResult response = request?.Fullcode switch
            {
                IfoodOrderStatusTypesExtensions.IFOOD_KEEPALIVE_FULL_CODE => Accepted(),

                IfoodOrderStatusTypesExtensions.IFOOD_PLACED_FULL_CODE => Accepted("/",
                    await ifoodToBizPik.OrderToBizPik(
                        new(
                           MerchantId: request.merchantId!,
                           OrderId: request.OrderId!,
                           CreatedAt: request.CreatedAt ?? DateTime.UtcNow
                        )
                    )
                ),

                IfoodOrderStatusTypesExtensions.IFOOD_CONFIRMED_FULL_CODE
                 or IfoodOrderStatusTypesExtensions.IFOOD_SEPARATION_STARTED_FULL_CODE
                 or IfoodOrderStatusTypesExtensions.IFOOD_SEPARATION_ENDED_FULL_CODE
                 or IfoodOrderStatusTypesExtensions.IFOOD_READY_TO_PICKUP_FULL_CODE
                 or IfoodOrderStatusTypesExtensions.IFOOD_DISPATCHED_FULL_CODE
                 or IfoodOrderStatusTypesExtensions.IFOOD_CONCLUDED_FULL_CODE
                 or IfoodOrderStatusTypesExtensions.IFOOD_CANCELLED_FULL_CODE
                 => Accepted( "/",
                     await ifoodToBizPik.ChangeOrderStatus(
                         new IfoodOrderChangeOrderStatusInput(
                             MerchantId: request.merchantId!,
                             OrderId: request.OrderId!,
                             Status: IfoodOrderStatusTypesExtensions.FromCode(request.Fullcode),
                             CreatedAt: request.CreatedAt ?? DateTime.UtcNow
                         )
                     )
                 ),

                _ => Ok(new { message = $"not mapped but ok {request?.Fullcode}" })
            };

            return response;
        });

        return routeGroup;
    }
}