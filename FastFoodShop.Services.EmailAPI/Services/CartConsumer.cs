using System.Text;
using FastFoodShop.Services.EmailAPI.Models.Dto;
using FastFoodShop.Services.EmailAPI.Utility;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace FastFoodShop.Services.EmailAPI.Services;

public class CartConsumer : BackgroundService
{
    private readonly IConnection _connection;
    private readonly IModel _channel;
    private readonly EmailService _emailService;

    public CartConsumer(EmailService emailService)
    {
        _emailService = emailService;
        var factory = new ConnectionFactory
        {
            HostName = "localhost",
            Password = "guest",
            UserName = "guest",
        };
        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();
        _channel.QueueDeclare(SD.QueueNameEmailShoppingCart, false, false, false, null);
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        stoppingToken.ThrowIfCancellationRequested();

        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += (ch, ea) =>
        {
            string content = Encoding.UTF8.GetString(ea.Body.ToArray());
            CartDto cartDto = JsonConvert.DeserializeObject<CartDto>(content);
            HandleMessage(cartDto).GetAwaiter().GetResult();
            _channel.BasicAck(ea.DeliveryTag, false);
        };

        _channel.BasicConsume(SD.QueueNameEmailShoppingCart, false, consumer);

        return Task.CompletedTask;
    }

    private async Task HandleMessage(CartDto cartDto)
    {
        await _emailService.EmailCartAndLog(cartDto);
    }
}