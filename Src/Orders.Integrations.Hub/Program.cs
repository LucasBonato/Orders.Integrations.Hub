using DotNetEnv;

using Orders.Integrations.Hub.Core;
using Orders.Integrations.Hub.Integrations;

using Scalar.AspNetCore;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

if (!builder.Environment.IsEnvironment("Test"))
    Env.TraversePath().Load();

builder.Services
    .AddCore(builder.Configuration)
    .AddIntegrationsModule(builder.Configuration)
;

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment()) {
    app.MapOpenApi().WithDocumentPerVersion();
    app.MapScalarApiReference();
}

await app
    .UseCore()
    .RunAsync();
