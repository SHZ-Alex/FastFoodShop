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

SD.QueueRegisterUser = builder.Configuration["TopicAndQueueNames:registeruser"];
SD.QueueNameEmailShoppingCart = builder.Configuration["TopicAndQueueNames:EmailShoppingCartQueue"];
SD.ServiceBusConnectionString = builder.Configuration["ServiceBusConnectionString"];

#region Database
builder.Services.AddDbContext<AppDbContext>(options => {
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnetion"));
});

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
// https://stackoverflow.com/questions/69961449/net6-and-datetime-problem-cannot-write-datetime-with-kind-utc-to-postgresql-ty
#endregion

DbContextOptionsBuilder<AppDbContext> optionBuilder = new DbContextOptionsBuilder<AppDbContext>();
optionBuilder.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
builder.Services.AddSingleton(new EmailService(optionBuilder.Options));

builder.Services.AddSingleton<IAzureServiceBusConsumer, AzureServiceBusConsumer>();




var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.UseAzureServiceBusConsumer();
app.Run();