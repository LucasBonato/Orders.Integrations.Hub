namespace Orders.Integrations.Hub.Integrations.Food99.Domain.ValueObjects.DTOs.Response;

public record Food99BaseResponse(
    int Errno,
    string Errmsg,
    string RequestId,
    long Time
) {
    public DateTime TimeUtc => DateTimeOffset.FromUnixTimeMilliseconds(Time).UtcDateTime;
};