using Bogus;

using Orders.Integrations.Hub.Core.Domain.Entity.Address;

namespace Orders.Integrations.Hub.TestCommon.Fakers.Order;

public sealed class AddressCoordinatesFaker : Faker<AddressCoordinates> {
    public AddressCoordinatesFaker() {
        CustomInstantiator(faker => new AddressCoordinates(
            Latitude: faker.Random.Decimal(-33.75m, 5.27m),
            Longitude: faker.Random.Decimal(-73.99m, -34.79m)
        ));
    }
}