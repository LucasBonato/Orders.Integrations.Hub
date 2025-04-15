using System.Text.Json.Serialization;

namespace Orders.Integrations.Hub.Modules.Common.Orders.Adapter.Dtos.Entity;

public record OrderSchedule(
    [property: JsonPropertyName("scheduledDateTimeStart")] DateTime ScheduledDateTimeStart,
    [property: JsonPropertyName("scheduledDateTimeEnd")] DateTime ScheduledDateTimeEnd
);