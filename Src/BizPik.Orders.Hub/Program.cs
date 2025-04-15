using BizPik.Orders.Hub.Modules.Integrations;

using FastEndpoints;

using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddOpenApi()
;

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseIntegrationsModule();
app.UseHttpsRedirection();
app.Run();