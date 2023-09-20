using System.Text;
using Azure.Messaging.ServiceBus;
using FastFoodShop.Services.RewardAPI.Models.Messages;
using FastFoodShop.Services.RewardAPI.Services;
using FastFoodShop.Services.RewardAPI.Utility;
using Newtonsoft.Json;

namespace FastFoodShop.Services.RewardAPI.Messaging;

public class AzureServiceBusConsumer : IAzureServiceBusConsumer
{
    private readonly ServiceBusProcessor _emailCartProcessor;
    private readonly ServiceBusProcessor _registerUserProcessor;
    private readonly RewardService _rewardService;
    private readonly ServiceBusProcessor _rewardProcessor;

    public AzureServiceBusConsumer(RewardService rewardService)
    {
        _rewardService = rewardService;
        
        ServiceBusClient client = new ServiceBusClient(SD.ServiceBusConnectionString);
        _rewardProcessor = client.CreateProcessor(SD.OrderCreatedTopic,SD.OrderCreatedRewardsSubscription);
    }

    public async Task Start()
    {
        _rewardProcessor.ProcessMessageAsync += OnNewOrderRewardsRequestReceived;
        _rewardProcessor.ProcessErrorAsync += ErrorHandler;
        await _rewardProcessor.StartProcessingAsync();
    }
    
    public async Task Stop()
    {
        await _rewardProcessor.StopProcessingAsync();
        await _rewardProcessor.DisposeAsync();
    }

    private async Task OnNewOrderRewardsRequestReceived(ProcessMessageEventArgs args)
    {
        ServiceBusReceivedMessage message = args.Message;
        string body = Encoding.UTF8.GetString(message.Body);

        RewardMessage objMessage = JsonConvert.DeserializeObject<RewardMessage>(body);
        try
        {
            await _rewardService.UpdateRewards(objMessage);  
            await args.CompleteMessageAsync(args.Message);
        }
        catch (Exception ex) {
            throw;
        }

    }

    private Task ErrorHandler(ProcessErrorEventArgs args)
    {
        Console.WriteLine(args.Exception.ToString());
        return Task.CompletedTask;
    }
}