namespace Orders.Integrations.Hub.Core.Application.Ports.Out.Serialization;

public interface ICustomJsonSerializer {
    string Serialize<T>(T value);
    T? Deserialize<T>(string value);
}