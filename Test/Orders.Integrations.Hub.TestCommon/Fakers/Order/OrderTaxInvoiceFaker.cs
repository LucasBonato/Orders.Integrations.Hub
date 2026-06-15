using Bogus;

using Orders.Integrations.Hub.Core.Domain.Entity;

namespace Orders.Integrations.Hub.TestCommon.Fakers.Order;

public sealed class OrderTaxInvoiceFaker : Faker<OrderTaxInvoice> {
    public OrderTaxInvoiceFaker() {
        CustomInstantiator(faker => new OrderTaxInvoice(
            Issued: faker.Random.Bool(),
            TaxInvoiceURL: faker.Internet.Url()
        ));
    }
}