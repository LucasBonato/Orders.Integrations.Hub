using BizPik.Orders.Hub.Modules.Common.Orders.Domain.Entity;

namespace BizPik.Orders.Hub.Modules.Common.Orders.Domain.Contracts;

public interface IOrderMapper<in TOrder>
{
    public Order Mapper(TOrder order);
}