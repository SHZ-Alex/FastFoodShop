using System.Text;
using Azure.Messaging.ServiceBus;
using FastFoodShop.Services.EmailAPI.Models.Dto;
using FastFoodShop.Services.EmailAPI.Services;
using FastFoodShop.Services.EmailAPI.Utility;
using Newtonsoft.Json;

namespace FastFoodShop.Services.EmailAPI.Messaging;

public class AzureServiceBusConsumer : IAzureServiceBusConsumer
{
    private readonly ServiceBusProcessor _emailCartProcessor;
    private readonly ServiceBusProcessor _registerUserProcessor;
    private readonly EmailService _emailService;

    public AzureServiceBusConsumer(EmailService emailService)
    {
        _emailService = emailService;
        
        ServiceBusClient client = new ServiceBusClient(SD.ServiceBusConnectionString);
        _emailCartProcessor = client.CreateProcessor(SD.QueueNameEmailShoppingCart);
        _registerUserProcessor = client.CreateProcessor(SD.QueueRegisterUser);
    }

    public async Task Start()
    {
        _emailCartProcessor.ProcessMessageAsync += OnEmailCartRequestReceived;
        _emailCartProcessor.ProcessErrorAsync += ErrorHandler;
        await _emailCartProcessor.StartProcessingAsync();
        
        _registerUserProcessor.ProcessMessageAsync += OnUserRegisterRequestReceived;
        _registerUserProcessor.ProcessErrorAsync += ErrorHandler;
        await _registerUserProcessor.StartProcessingAsync();
    }

    private async Task OnUserRegisterRequestReceived(ProcessMessageEventArgs args)
    {
        var message = args.Message;
        var body = Encoding.UTF8.GetString(message.Body);

        string email = JsonConvert.DeserializeObject<string>(body);
        
        await _emailService.RegisterUserEmailAndLog(email);
        await args.CompleteMessageAsync(args.Message);
    }


    public async Task Stop()
    {
        await _emailCartProcessor.StopProcessingAsync();
        await _emailCartProcessor.DisposeAsync();
        
        await _registerUserProcessor.StopProcessingAsync();
        await _registerUserProcessor.DisposeAsync();
    }

    private async Task OnEmailCartRequestReceived(ProcessMessageEventArgs args)
    {
        ServiceBusReceivedMessage message = args.Message;
        string body = Encoding.UTF8.GetString(message.Body);

        CartDto objMessage = JsonConvert.DeserializeObject<CartDto>(body);
        
        await _emailService.EmailCartAndLog(objMessage);  
        await args.CompleteMessageAsync(args.Message);
        
    }

    private Task ErrorHandler(ProcessErrorEventArgs args)
    {
        Console.WriteLine(args.Exception.ToString());
        return Task.CompletedTask;
    }
}