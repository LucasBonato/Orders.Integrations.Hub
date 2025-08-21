namespace Orders.Integrations.Hub.Core.Domain.Entity;

public record OrderSchedule(
    DateTime ScheduledDateTimeStart,
    DateTime ScheduledDateTimeEnd
);