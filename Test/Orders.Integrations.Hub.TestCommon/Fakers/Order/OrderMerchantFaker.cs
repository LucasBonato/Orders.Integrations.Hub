using Bogus;

using Orders.Integrations.Hub.Core.Domain.Entity.Merchant;

namespace Orders.Integrations.Hub.TestCommon.Fakers.Order;

public sealed class OrderMerchantFaker : Faker<OrderMerchant> {
    public OrderMerchantFaker() {
        CustomInstantiator(faker => new OrderMerchant(
            Id: faker.Random.Guid().ToString(),
            Name: faker.Company.CompanyName()
        ));
    }
}