using Microsoft.AspNetCore.Mvc;

using Orders.Integrations.Hub.Core.Adapters.In.Http.Endpoint;
using Orders.Integrations.Hub.Core.Application.Ports.In.UseCases;
using Orders.Integrations.Hub.Integrations.Common.Middleware;
using Orders.Integrations.Hub.Integrations.IFood.Domain.ValueObjects.DTOs.Request;
using Orders.Integrations.Hub.Integrations.IFood.Domain.ValueObjects.Enums;
using Orders.Integrations.Hub.Integrations.IFood.Infrastructure;

using static Microsoft.AspNetCore.Http.Results;

namespace Orders.Integrations.Hub.Integrations.IFood.Adapter;

internal sealed class IFoodWebhookEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/IFood/Webhook", async (
            [FromServices] IOrderCreateUseCase<IFoodWebhookRequest> orderCreate,
            [FromServices] IOrderUpdateUseCase<IFoodWebhookRequest> orderUpdate,
            [FromServices] IOrderDisputeUseCase<IFoodWebhookRequest> orderDispute,
            ILogger<IFoodWebhookEndpoint> logger,
            HttpContext context
        ) => {
            IFoodWebhookRequest request = (IFoodWebhookRequest)context.Items["WebhookRequest"]!;

            if (request.FullCode != IFoodFullOrderStatus.KEEPALIVE && logger.IsEnabled(LogLevel.Information))
                logger.LogInformation("[INFO] - IFoodAdapter - IFood Webhook code: {FullCode}", request.FullCode);

            return request.FullCode switch {
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
        })
        .WithTags("IFood")
        .WithDescription("IFood Webhook Endpoint")
        .Produces<IFoodWebhookRequest>()
        .ProducesValidationProblem()
        .AddEndpointFilter<WebhookSignatureFilter<IFoodWebhookRequest, IFoodSignatureStrategy, IFoodSignatureStrategy>>();
    }
}