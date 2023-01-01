using Azure.Messaging.ServiceBus;
using Mango.MessageBus;
using Mango.OrderApi.Dtos;
using System.Text.Json;

namespace Mango.OrderApi.Services;

public class CheckoutQueueReceiver : AzureMessageBusReceiver
{
    private readonly IServiceProvider _serviceProvider;

    public CheckoutQueueReceiver(
        IServiceProvider serviceProvider,
        string queueName
        )
        : base(
            serviceProvider.GetRequiredService<ILogger<CheckoutQueueReceiver>>(),
            serviceProvider.GetRequiredService<ServiceBusClient>(),
            queueName,
            null)
    {
        _serviceProvider = serviceProvider;
    }

    // handle received messages
    protected override async Task MessageHandler(ProcessMessageEventArgs args)
    {
        using var scope = _serviceProvider.CreateScope();
        var repository = scope.ServiceProvider.GetRequiredService<IOrderService>();

        var checkout = JsonSerializer.Deserialize<CheckoutDto>(args.Message.Body.ToString());
        await repository.CreateOrder(checkout!);

        // complete the message. messages is deleted from the subscription.
        await args.CompleteMessageAsync(args.Message);
    }
}