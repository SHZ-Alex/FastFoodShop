namespace FastFoodShop.Services.ShoppingCartAPI.Service.IService;

public interface IAuthMessageSender
{
    void SendMessage(Object message, string queueName);
}