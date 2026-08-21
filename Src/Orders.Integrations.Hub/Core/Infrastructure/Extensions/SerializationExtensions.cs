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
        services.Configure<JsonOptions>(options => ConfigureJsonSerializer(options.JsonSerializerOptions));
        services.ConfigureHttpJsonOptions(options => ConfigureJsonSerializer(options.SerializerOptions));
        return services;
    }

    private static void ConfigureJsonSerializer(JsonSerializerOptions options) {
        options.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        options.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
        options.PropertyNameCaseInsensitive = true;
        options.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.SnakeCaseUpper));
        options.Converters.Add(new IntegrationKeyJsonConverter());
    }
}