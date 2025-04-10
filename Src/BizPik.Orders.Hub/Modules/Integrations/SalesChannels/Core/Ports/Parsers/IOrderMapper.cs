using BizPik.Orders.Hub.Modules.Common.Orders.Adapter.Dtos.Entity;

namespace BizPik.Orders.Hub.Modules.Integrations.SalesChannels.Core.Ports.Parsers;

public interface IOrderMapper<T>
{
    Order Parse(T integrationOrder);
}