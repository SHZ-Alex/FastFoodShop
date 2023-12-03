using FastFoodShop.Services.EmailAPI.Data;
using FastFoodShop.Services.EmailAPI.Extension;
using FastFoodShop.Services.EmailAPI.Messaging;
using FastFoodShop.Services.EmailAPI.Services;
using FastFoodShop.Services.EmailAPI.Utility;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

SD.QueueRegisterUser = builder.Configuration["TopicAndQueueNames:RegisterUserQueue"];
SD.QueueNameEmailShoppingCart = builder.Configuration["TopicAndQueueNames:EmailShoppingCartQueue"];
SD.ServiceBusConnectionString = builder.Configuration["ServiceBusConnectionString"];
SD.OrderCreatedTopic = builder.Configuration["TopicAndQueueNames:OrderCreatedTopic"];
SD.OrderCreatedEmailSubscription = builder.Configuration["TopicAndQueueNames:OrderCreatedEmailSubscription"];

builder.Services.AddDbContext<AppDbContext>(option =>
    { option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnetion")); });

DbContextOptionsBuilder<AppDbContext> optionBuilder = new DbContextOptionsBuilder<AppDbContext>();
optionBuilder.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnetion"));
builder.Services.AddSingleton(new EmailService(optionBuilder.Options));

//builder.Services.AddSingleton<IAzureServiceBusConsumer, AzureServiceBusConsumer>();
builder.Services.AddHostedService<AuthConsumer>();
builder.Services.AddHostedService<CartConsumer>();

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
//app.UseAzureServiceBusConsumer();
app.ApplyMigration();
app.Run();