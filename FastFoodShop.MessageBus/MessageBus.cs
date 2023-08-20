using System.Text;
using Azure.Messaging.ServiceBus;
using Newtonsoft.Json;

namespace FastFoodShop.MessageBus;

public class MessageBus : IMessageBus
{
    private const string connectionString =
        "Endpoint=sb://fastfoodshop.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=rII3ILNvUM1zL4/jHrquTYkyQIGysIWyQ+ASbPC653o=";

    public async Task PublishMessage(object message, string topicQueueName)
    {
        await using ServiceBusClient client = new ServiceBusClient(connectionString);

        ServiceBusSender sender = client.CreateSender(topicQueueName);

        string jsonMessage = JsonConvert.SerializeObject(message);
        ServiceBusMessage finalMessage = new ServiceBusMessage(Encoding
            .UTF8.GetBytes(jsonMessage))
        {
            CorrelationId = Guid.NewGuid().ToString(),
        };

        await sender.SendMessageAsync(finalMessage);
        await client.DisposeAsync();
    }
}