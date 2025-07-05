using DotNetEnv;

using Orders.Integrations.Hub.Core;
using Orders.Integrations.Hub.Integrations;

using Scalar.AspNetCore;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

Env.TraversePath().Load();

builder.Services
    .AddOpenApi()
    .AddCore()
    .AddIntegrationsModule()
;

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseCore();
app.UseIntegrationsModule();
app.Run();