using Orders.Integrations.Hub.Modules.Common.Orders.Domain.Entity;

namespace Orders.Integrations.Hub.Modules.Common.Orders.Domain.Contracts;

public interface IOrderMapper<in TOrder>
{
    public Order Mapper(TOrder order);
}