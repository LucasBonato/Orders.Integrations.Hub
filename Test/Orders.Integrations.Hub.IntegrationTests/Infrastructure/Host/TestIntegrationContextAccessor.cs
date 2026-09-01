using Orders.Integrations.Hub.Integrations.Common.Application;

namespace Orders.Integrations.Hub.IntegrationTests.Infrastructure.Host;

public sealed class TestIntegrationContextAccessor
{
    public IntegrationContext? Current { get; private set; }

    public void Set(IntegrationContext context) {
        Current = context;
    }
}