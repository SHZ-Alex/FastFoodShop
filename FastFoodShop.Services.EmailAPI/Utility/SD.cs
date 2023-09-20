namespace FastFoodShop.Services.EmailAPI.Utility;

public class SD
{
    public static string QueueNameEmailShoppingCart { get; set; }
    public static string QueueRegisterUser { get; set; }
    public static string ServiceBusConnectionString { get; set; }
    public static string OrderCreatedTopic { get; set; }
    public static string OrderCreatedEmailSubscription { get; set; }
}