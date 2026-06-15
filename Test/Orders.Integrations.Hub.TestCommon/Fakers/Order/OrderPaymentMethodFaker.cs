using Bogus;

using Orders.Integrations.Hub.Core.Domain.Entity.Payment;
using Orders.Integrations.Hub.Core.Domain.Enums;

namespace Orders.Integrations.Hub.TestCommon.Fakers.Order;

public sealed class OrderPaymentMethodFaker : Faker<OrderPaymentMethod> {
    public OrderPaymentMethodFaker() {
        CustomInstantiator(faker => new OrderPaymentMethod(
            Value: faker.Random.Decimal(1m, 999.99m),
            Currency: "BRL",
            Type: faker.PickRandom<MethodType>(),
            Method: faker.PickRandom<MethodMethod>(),
            Brand: faker.PickRandom<MethodBrand>(),
            MethodInfo: faker.Random.AlphaNumeric(10),
            Transaction: faker.Random.Bool() ? new OrderPaymentMethodTransactionFaker().Generate() : null,
            ChangeFor: faker.Random.Bool() ? faker.Random.Decimal(1m, 100m) : null
        ));
    }
}