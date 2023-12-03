namespace FastFoodShop.Services.AuthAPI.Service.IService;

public interface IAuthMessageSender
{
    void SendMessage(Object message, string queueName);
}