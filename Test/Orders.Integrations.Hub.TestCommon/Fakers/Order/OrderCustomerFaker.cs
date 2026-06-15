using Bogus;

using Orders.Integrations.Hub.Core.Domain.Entity;

namespace Orders.Integrations.Hub.TestCommon.Fakers.Order;

public sealed class OrderCustomerFaker : Faker<OrderCustomer> {
    public OrderCustomerFaker() {
        CustomInstantiator(faker => new OrderCustomer(
            Id: faker.Random.Guid().ToString(),
            Name: faker.Name.FullName(),
            DocumentNumber: faker.Random.Replace("###.###.###-##"),
            Email: faker.Random.Bool() ? faker.Internet.Email() : null,
            Phone: new PhoneFaker().Generate(),
            OrdersCountOnMerchant: faker.Random.Int(0, 100)
        ));
    }
}