using Orders.Integrations.Hub.Integrations.Common.Contracts;
using Orders.Integrations.Hub.Integrations.Food99.Domain.ValueObjects.DTOs.Request;

using Refit;

namespace Orders.Integrations.Hub.Integrations.Food99.Application.Clients;

public interface IFood99Client : IIntegrationClient {
    [Post("/v1/order/order/confirm")]
    Task ConfirmOrder([Body] Food99StatusChangeRequest request);

    [Post("/v1/order/order/ready")]
    Task ReadyToPickupOrder([Body] Food99StatusChangeRequest request);

    [Post("/v1/order/order/delivered")]
    Task DeliveredOrder([Body] Food99StatusChangeRequest request);

    [Post("/v1/order/order/cancel")]
    Task CancelOrder([Body] Food99StatusChangeRequest request);
}
