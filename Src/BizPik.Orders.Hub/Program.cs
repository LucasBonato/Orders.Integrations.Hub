using BizPik.Orders.Hub.Modules.Core;
using BizPik.Orders.Hub.Modules.Integrations;

using DotNetEnv;

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