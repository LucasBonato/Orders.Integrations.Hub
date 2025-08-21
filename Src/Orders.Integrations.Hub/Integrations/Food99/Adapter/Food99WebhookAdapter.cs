using System.Text;

using Microsoft.AspNetCore.Mvc;

using Orders.Integrations.Hub.Core.Application.Extensions;
using Orders.Integrations.Hub.Core.Domain.Contracts;
using Orders.Integrations.Hub.Core.Domain.Contracts.UseCases.Integrations.In;
using Orders.Integrations.Hub.Core.Domain.ValueObjects.Enums;
using Orders.Integrations.Hub.Integrations.Common.Contracts;
using Orders.Integrations.Hub.Integrations.Common.Validators;
using Orders.Integrations.Hub.Integrations.Food99.Domain.Entity;
using Orders.Integrations.Hub.Integrations.Food99.Domain.ValueObjects.Enums;

using static Microsoft.AspNetCore.Http.Results;

namespace Orders.Integrations.Hub.Integrations.Food99.Adapter;

public class Food99WebhookAdapter
{
    public static async Task<IResult> Webhook(
        [FromKeyedServices(OrderIntegration.FOOD99)] ICustomJsonSerializer serializer,
        [FromServices] IOrderCreateUseCase<Food99WebhookRequest> orderCreate,
        [FromServices] IOrderUpdateUseCase<Food99WebhookRequest> orderUpdate,
        [FromServices] IOrderDisputeUseCase<Food99WebhookRequest> orderDispute,
        [FromServices] IServiceProvider serviceProvider,
        [FromServices] IIntegrationContext integrationContext,
        ILogger<Food99WebhookAdapter> logger,
        HttpContext context
    ) {
        Food99WebhookRequest request = await HandleSignature(integrationContext, serializer, logger, context);

        Food99WebhookRequest? result = request.Type switch {
            Food99Type.OrderNew => await orderCreate.ExecuteAsync(request),

            Food99Type.DeliveryStatus or
            Food99Type.OrderCancel or
            Food99Type.OrderPartialCancel or
            Food99Type.OrderFinish => await orderUpdate.ExecuteAsync(request),

            Food99Type.OrderCancelApply or
            Food99Type.OrderRefundApply => await orderDispute.ExecuteAsync(request),
            _ => null
        };

        return result is null
            ? BadRequest(new { Errno = 1, Errmsg = "Could not detect webhook request" })
            : Ok(new { Errno = 0, Errmsg = "ok" });
    }

    /// <summary>
    /// This will valid the signature <c>Food99-Signature</c>
    /// </summary>
    /// <param name="integrationContext">The context of the integration</param>
    /// <param name="serializer">A serializer with food99 configs</param>
    /// <param name="logger">A logger</param>
    /// <param name="context">The HttpContext of the request</param>
    /// <returns>The Body of the request formatted as a Food99Order</returns>
    private static async Task<Food99WebhookRequest> HandleSignature(
        IIntegrationContext integrationContext,
        ICustomJsonSerializer serializer,
        ILogger<Food99WebhookAdapter> logger,
        HttpContext context
    ) {
        string body;
        using (var reader = new StreamReader(context.Request.Body, Encoding.UTF8, detectEncodingFromByteOrderMarks: false, leaveOpen: true)) {
            body = await reader.ReadToEndAsync();
        }

        string? signature = context.Request.Headers["didi-header-sign"].FirstOrDefault();

        Food99WebhookRequest request = serializer.Deserialize<Food99WebhookRequest>(body)!;

        logger.LogInformation("[INFO] - 99FoodSignatureValidator - Request body: {body}", body);

        // integrationContext.Integration = await internalClient.GetIntegrationByExternalId(request.AppShopId);
        integrationContext.MerchantId = request.AppShopId;

        string secret = AppEnv.INTEGRATIONS.FOOD99.CLIENT.SECRET.NotNullEnv();

        if (signature == null) {
            context.Response.StatusCode = 401;
            logger.LogWarning("[WARN] - Signature Header is missing.");
            throw new Exception("Missing didi-header-sign");
        }

        if (!signature.IsValidateMd5Signature(body, secret)) {
            context.Response.StatusCode = 401;
            logger.LogWarning("[WARN] - Invalid signature.");
            throw new Exception("Invalid signature");
        }

        return request;
    }
}