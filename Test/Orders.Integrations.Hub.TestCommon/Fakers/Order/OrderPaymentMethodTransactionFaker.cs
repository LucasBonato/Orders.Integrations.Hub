using Bogus;

using Orders.Integrations.Hub.Core.Domain.Entity.Payment;

namespace Orders.Integrations.Hub.TestCommon.Fakers.Order;

public sealed class OrderPaymentMethodTransactionFaker : Faker<OrderPaymentMethodTransaction> {
    public OrderPaymentMethodTransactionFaker() {
        CustomInstantiator(faker => new OrderPaymentMethodTransaction(
            AuthorizationCode: faker.Random.AlphaNumeric(12).ToUpper(),
            AcquirerDocument: faker.Random.Replace("##.###.###/####-##")
        ));
    }
}