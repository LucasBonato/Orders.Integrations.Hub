using Orders.Integrations.Hub.Integrations.Common.Contracts;

namespace Orders.Integrations.Hub.Integrations.Food99.Domain.Contracts;

public interface IFood99Client : IIntegrationClient
{
    Task ConfirmOrder(string appShopId, string orderId);
    Task CancelOrder(string appShopId, string orderId, string reason, int reasonId);
    Task ReadyToPickupOrder(string appShopId, string orderId);
    Task DeliveredOrder(string appShopId, string orderId);
}