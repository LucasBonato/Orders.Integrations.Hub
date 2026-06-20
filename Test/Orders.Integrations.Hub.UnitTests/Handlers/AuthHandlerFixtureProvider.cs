using Orders.Integrations.Hub.UnitTests.Handlers.Fixtures;

namespace Orders.Integrations.Hub.UnitTests.Handlers;

public sealed class AuthHandlerFixtureProvider : TheoryData<AuthHandlerTestFixture>
{
    public AuthHandlerFixtureProvider()
    {
        Add(new IFoodAuthHandlerFixture());
        Add(new RappiAuthHandlerFixture());
        Add(new Food99AuthHandlerFixture());
    }
}
