using System.Text;

using Microsoft.AspNetCore.Mvc;

using Orders.Integrations.Hub.Core.Application.Extensions;
using Orders.Integrations.Hub.Core.Domain.Contracts;
using Orders.Integrations.Hub.Core.Domain.Contracts.UseCases;
using Orders.Integrations.Hub.Core.Domain.Contracts.UseCases.Integrations.In;
using Orders.Integrations.Hub.Core.Domain.ValueObjects.Enums;
using Orders.Integrations.Hub.Integrations.Common.Validators;
using Orders.Integrations.Hub.Integrations.Ifood.Domain.ValueObjects.DTOs.Request;
using Orders.Integrations.Hub.Integrations.Ifood.Domain.ValueObjects.Enums;

using static Microsoft.AspNetCore.Http.Results;

namespace Orders.Integrations.Hub.Integrations.Ifood.Adapter;

public class IFoodAdapter
{
    public static async Task<IResult> Webhook(
        [FromKeyedServices(OrderIntegration.IFOOD)] ICustomJsonSerializer jsonSerializer,
        [FromServices] IOrderCreateUseCase<IfoodWebhookRequest> orderCreate,
        [FromServices] IOrderUpdateUseCase<IfoodWebhookRequest> orderUpdate,
        [FromServices] IOrderDisputeUseCase<IfoodWebhookRequest> orderDispute,
        ILogger<IFoodAdapter> logger,
        HttpContext context
    ) {
        IfoodWebhookRequest request = await HandleSignature(jsonSerializer, logger, context);

        if (request.FullCode != IfoodFullOrderStatus.KEEPALIVE)
            logger.LogInformation("[INFO] - IFoodAdapter - Ifood Webhook code: {FullCode}", request.FullCode);

        return request.FullCode switch
        {
            IfoodFullOrderStatus.KEEPALIVE => Accepted(),

            IfoodFullOrderStatus.PLACED => Accepted(value: await orderCreate.ExecuteAsync(request)),

            IfoodFullOrderStatus.CONFIRMED or
            IfoodFullOrderStatus.SEPARATION_STARTED or
            IfoodFullOrderStatus.SEPARATION_ENDED or
            IfoodFullOrderStatus.READY_TO_PICKUP or
            IfoodFullOrderStatus.DISPATCHED or
            IfoodFullOrderStatus.CONCLUDED or
            IfoodFullOrderStatus.CANCELLED => Accepted(value: await orderUpdate.ExecuteAsync(request)),

            IfoodFullOrderStatus.HANDSHAKE_DISPUTE or
            IfoodFullOrderStatus.HANDSHAKE_SETTLEMENT => Accepted(value: await orderDispute.ExecuteAsync(request)),

            _ => BadRequest(new { error = $"not mapped but ok {request.FullCode}" })
        };
    }

    private static async Task<IfoodWebhookRequest> HandleSignature(
        ICustomJsonSerializer jsonSerializer,
        ILogger<IFoodAdapter> logger,
        HttpContext context
    ) {
        string body;
        using (var reader = new StreamReader(context.Request.Body, Encoding.UTF8, detectEncodingFromByteOrderMarks: false, leaveOpen: true))
        {
            body = await reader.ReadToEndAsync();
        }

        string? signature = context.Request.Headers["X-Ifood-Signature"].FirstOrDefault();

        IfoodWebhookRequest request = jsonSerializer.Deserialize<IfoodWebhookRequest>(body)!;

        if (request.FullCode == IfoodFullOrderStatus.KEEPALIVE)
            return request;

        logger.LogInformation("[INFO] - IfoodSignatureValidator - Request Body: {body}", body);

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