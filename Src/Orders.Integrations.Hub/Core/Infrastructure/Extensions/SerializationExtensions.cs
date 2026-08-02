using System.Text.Json;
using System.Text.Json.Serialization;

using Microsoft.AspNetCore.Mvc;

using Orders.Integrations.Hub.Core.Application.Ports.Out.Serialization;
using Orders.Integrations.Hub.Core.Infrastructure.Serialization;

namespace Orders.Integrations.Hub.Core.Infrastructure.Extensions;

public static class SerializationExtensions
{
    public static IServiceCollection AddSerializationConfiguration(this IServiceCollection services) {
        services.AddSingleton<ICustomJsonSerializer, CoreJsonSerializer>();
        services.Configure<JsonOptions>(options => {
            options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            options.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
            options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
            options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.SnakeCaseUpper));
        });
        services.ConfigureHttpJsonOptions(options => {
            options.SerializerOptions.Converters.Add(new IntegrationKeyJsonConverter());
        });
        return services;
    }
}