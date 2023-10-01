using FastFoodShop.Services.RewardAPI.Data;
using FastFoodShop.Services.RewardAPI.Extension;
using FastFoodShop.Services.RewardAPI.Messaging;
using FastFoodShop.Services.RewardAPI.Services;
using FastFoodShop.Services.RewardAPI.Utility;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(option =>
    { option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnetion")); });

SD.OrderCreatedTopic = builder.Configuration["TopicAndQueueNames:OrderCreatedTopic"];
SD.OrderCreatedRewardsSubscription = builder.Configuration["TopicAndQueueNames:OrderCreatedRewardsSubscription"];
SD.ServiceBusConnectionString = builder.Configuration["ServiceBusConnectionString"];

DbContextOptionsBuilder<AppDbContext> optionBuilder = new DbContextOptionsBuilder<AppDbContext>();
optionBuilder.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnetion"));

builder.Services.AddSingleton(new RewardService(optionBuilder.Options));
builder.Services.AddSingleton<IAzureServiceBusConsumer, AzureServiceBusConsumer>();


var app = builder.Build();

// Configure the HTTP request pipeline.
    app.UseSwagger();
app.UseSwaggerUI(x =>
{
    x.SwaggerEndpoint("/swagger/v1/swagger.json", "AUTH API");
    x.RoutePrefix = string.Empty;
});


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.UseAzureServiceBusConsumer();
app.ApplyMigration();
app.Run();