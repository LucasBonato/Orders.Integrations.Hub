using Bogus;

using Orders.Integrations.Hub.Core.Domain.Entity.Item;
using Orders.Integrations.Hub.Core.Domain.Enums;

namespace Orders.Integrations.Hub.TestCommon.Fakers.Order;

public sealed class OrderItemFaker : Faker<OrderItem> {
    public OrderItemFaker() {
        CustomInstantiator(faker => new OrderItem(
            Id: faker.Random.Guid().ToString(),
            Index: faker.IndexFaker,
            Name: faker.Commerce.ProductName(),
            ExternalCode: faker.Random.AlphaNumeric(8).ToUpper(),
            ImageUrl: faker.Image.PicsumUrl(),
            Unit: faker.PickRandom<OrderUnit>(),
            Ean: faker.Random.Replace("##############"),
            Quantity: faker.Random.Int(1, 10),
            SpecialInstructions: faker.Random.Bool() ? faker.Lorem.Sentence() : string.Empty,
            UnitPrice: new PriceFaker().Generate(),
            TotalPrice: new PriceFaker().Generate(),
            OptionsPrice: new PriceFaker().Generate(),
            Options: new OrderItemOptionFaker().Generate(faker.Random.Int(0, 3))
        ));
    }
}