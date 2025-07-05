namespace Orders.Integrations.Hub.Core.Domain.Contracts;

public interface ICustomJsonSerializer {
    string Serialize<T>(T value);
    T? Deserialize<T>(string value);
}