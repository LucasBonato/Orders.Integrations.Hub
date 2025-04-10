using Orders.Integrations.Hub.Modules.Common.Orders.Adapter.Dtos.Entity;

namespace Orders.Integrations.Hub.Modules.Integrations.SalesChannels.Core.Ports.Parsers;

public interface IOrderMapper<T>
{
    Order Parse(T integrationOrder);
}