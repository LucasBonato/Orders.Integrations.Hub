using System.Reflection;
using System.Text;

using Orders.Integrations.Hub.Core.Application.Ports.Out.Serialization;

using Refit;

namespace Orders.Integrations.Hub.Integrations.Common.Serialization;

public class CustomJsonContentSerializer(
    ICustomJsonSerializer serializer
) : IHttpContentSerializer {
    public HttpContent ToHttpContent<T>(T item)
    {
        string json = serializer.Serialize(item);
        return new StringContent(json, Encoding.UTF8, "application/json");
    }

    public async Task<T?> FromHttpContentAsync<T>(HttpContent content, CancellationToken cancellationToken = default)
    {
        string json = await content.ReadAsStringAsync(cancellationToken);
        return serializer.Deserialize<T>(json);
    }

    public string? GetFieldNameForProperty(PropertyInfo propertyInfo) => null;
}
