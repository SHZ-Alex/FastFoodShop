namespace FastFoodShop.Services.EmailAPI.Messaging;

public interface IAzureServiceBusConsumer
{
    Task Start();
    Task Stop();
}