using System.Text.Json.Serialization;

namespace BizPik.Orders.Hub.Core.Orders.Domain.Entity;

public record OrderSchedule(
    [property: JsonPropertyName("scheduledDateTimeStart")] DateTime ScheduledDateTimeStart,
    [property: JsonPropertyName("scheduledDateTimeEnd")] DateTime ScheduledDateTimeEnd
);