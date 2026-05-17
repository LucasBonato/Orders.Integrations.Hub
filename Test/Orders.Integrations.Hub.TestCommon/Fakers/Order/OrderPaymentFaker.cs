using Bogus;

using Orders.Integrations.Hub.Core.Domain.Entity.Payment;

namespace Orders.Integrations.Hub.TestCommon.Fakers.Order;

public sealed class OrderPaymentFaker : Faker<OrderPayment> {
    public OrderPaymentFaker() {
        CustomInstantiator(faker => new OrderPayment(
            Prepaid: faker.Random.Int(0, 1),
            Pending: faker.Random.Decimal(0m, 99.99m),
            Methods: new OrderPaymentMethodFaker().Generate(faker.Random.Int(1, 3))
        ));
    }
}