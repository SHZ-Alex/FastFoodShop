using System.Text;
using FastFoodShop.Services.ShoppingCartAPI.Service.IService;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace FastFoodShop.Services.ShoppingCartAPI.Service;

public class AuthMessageSender : IAuthMessageSender
{
    private const string _hostName = "localhost";
    private const string _username = "guest";
    private const string _password = "guest";
    private IConnection _connection;

    public void SendMessage(object message, string queueName)
    {
        if (!ConnectionExists()) 
            return;
        
        using var channel = _connection.CreateModel();
        channel.QueueDeclare(queueName, false, false, false, null);
        var json = JsonConvert.SerializeObject(message);
        var body = Encoding.UTF8.GetBytes(json);
        channel.BasicPublish(exchange: "", routingKey: queueName, null, body: body);
    }
    
    private void CreateConnection()
    {
        try
        {
            var factory = new ConnectionFactory
            {
                HostName = _hostName,
                Password = _password,
                UserName = _username
            };

            _connection = factory.CreateConnection();
        }
        catch (Exception ex)
        {
            // ignored
        }
    }

    private bool ConnectionExists()
    {
        if (_connection != null)
        {
            return true;
        }
        CreateConnection();
        return true;
    }
}