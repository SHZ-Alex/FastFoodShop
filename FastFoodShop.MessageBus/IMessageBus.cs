namespace FastFoodShop.MessageBus;

public interface IMessageBus
{
    Task PublishMessage(object message, string topicQueueName);
}