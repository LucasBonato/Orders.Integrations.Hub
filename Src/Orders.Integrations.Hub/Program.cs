using DotNetEnv;

using Orders.Integrations.Hub.Core;
using Orders.Integrations.Hub.Integrations;

using Scalar.AspNetCore;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

if (!string.Equals(Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"), "Test", StringComparison.OrdinalIgnoreCase))
    Env.TraversePath().Load();

builder.Services
    .AddCore()
    .AddIntegrationsModule()
    .AddOpenApi()
;

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment()) {
    app.MapOpenApi();
    app.MapScalarApiReference();
}

await app
    .UseCore()
    .RunAsync();