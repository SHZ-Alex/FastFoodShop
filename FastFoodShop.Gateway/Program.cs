using FastFoodShop.Gateway.Extensions;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);
builder.AddAppAuthetication();
builder.Configuration.AddJsonFile(
    builder.Environment.EnvironmentName.ToLower().Equals("production")
        ? "ocelot.Production.json"
        : "ocelot.json", optional: false, reloadOnChange: true);

builder.Services.AddOcelot(builder.Configuration);

var app = builder.Build();

app.MapGet("/", () => "Hello World!");
app.UseOcelot().GetAwaiter().GetResult();
app.Run();