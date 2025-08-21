using System.Text;

using Microsoft.AspNetCore.Mvc;

using Orders.Integrations.Hub.Core.Application.Extensions;
using Orders.Integrations.Hub.Core.Domain.Contracts;
using Orders.Integrations.Hub.Core.Domain.Contracts.UseCases.Integrations.In;
using Orders.Integrations.Hub.Core.Domain.ValueObjects.Enums;
using Orders.Integrations.Hub.Integrations.Common.Validators;
using Orders.Integrations.Hub.Integrations.IFood.Domain.ValueObjects.DTOs.Request;
using Orders.Integrations.Hub.Integrations.IFood.Domain.ValueObjects.Enums;

using static Microsoft.AspNetCore.Http.Results;

namespace Orders.Integrations.Hub.Integrations.IFood.Adapter;

public class IFoodAdapter
{
    public static async Task<IResult> Webhook(
        [FromKeyedServices(OrderIntegration.IFOOD)] ICustomJsonSerializer jsonSerializer,
        [FromServices] IOrderCreateUseCase<IFoodWebhookRequest> orderCreate,
        [FromServices] IOrderUpdateUseCase<IFoodWebhookRequest> orderUpdate,
        [FromServices] IOrderDisputeUseCase<IFoodWebhookRequest> orderDispute,
        ILogger<IFoodAdapter> logger,
        HttpContext context
    ) {
        IFoodWebhookRequest request = await HandleSignature(jsonSerializer, logger, context);

        if (request.FullCode != IFoodFullOrderStatus.KEEPALIVE)
            logger.LogInformation("[INFO] - IFoodAdapter - IFood Webhook code: {FullCode}", request.FullCode);

        return request.FullCode switch
        {
            IFoodFullOrderStatus.KEEPALIVE => Accepted(),

            IFoodFullOrderStatus.PLACED => Accepted(value: await orderCreate.ExecuteAsync(request)),

            IFoodFullOrderStatus.CONFIRMED or
            IFoodFullOrderStatus.SEPARATION_STARTED or
            IFoodFullOrderStatus.SEPARATION_ENDED or
            IFoodFullOrderStatus.READY_TO_PICKUP or
            IFoodFullOrderStatus.DISPATCHED or
            IFoodFullOrderStatus.CONCLUDED or
            IFoodFullOrderStatus.CANCELLED => Accepted(value: await orderUpdate.ExecuteAsync(request)),

            IFoodFullOrderStatus.HANDSHAKE_DISPUTE or
            IFoodFullOrderStatus.HANDSHAKE_SETTLEMENT => Accepted(value: await orderDispute.ExecuteAsync(request)),

            _ => BadRequest(new { error = $"not mapped but ok {request.FullCode}" })
        };
    }

    private static async Task<IFoodWebhookRequest> HandleSignature(
        ICustomJsonSerializer jsonSerializer,
        ILogger<IFoodAdapter> logger,
        HttpContext context
    ) {
        string body;
        using (var reader = new StreamReader(context.Request.Body, Encoding.UTF8, detectEncodingFromByteOrderMarks: false, leaveOpen: true))
        {
            body = await reader.ReadToEndAsync();
        }

        string? signature = context.Request.Headers["X-IFood-Signature"].FirstOrDefault();

        IFoodWebhookRequest request = jsonSerializer.Deserialize<IFoodWebhookRequest>(body)!;

        if (request.FullCode == IFoodFullOrderStatus.KEEPALIVE)
            return request;

        logger.LogInformation("[INFO] - IFoodSignatureValidator - Request Body: {body}", body);

        string secret = AppEnv.INTEGRATIONS.IFOOD.CLIENT.SECRET.NotNullEnv();

        if (signature == null)
        {
            logger.LogWarning("[WARN] - Signature header is missing.");
            throw new Exception("Signature header is missing.");
        }

        if (!signature.IsSignatureValid(secret, body))
        {
            logger.LogWarning("[WARN] - Invalid signature.");
            throw new Exception("Invalid signature");
        }

        return request;
    }
}