using Bogus;

using Orders.Integrations.Hub.Core.Domain.Entity;

namespace Orders.Integrations.Hub.TestCommon.Fakers.Order;

public sealed class OrderScheduleFaker : Faker<OrderSchedule> {
    public OrderScheduleFaker() {
        CustomInstantiator(faker => {
            DateTime start = faker.Date.Soon();
            return new OrderSchedule(
                ScheduledDateTimeStart: start,
                ScheduledDateTimeEnd: start.AddMinutes(faker.Random.Int(30, 120))
            );
        });
    }
}