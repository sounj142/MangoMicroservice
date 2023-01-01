using Azure.Messaging.ServiceBus;
using Mango.MessageBus;
using Mango.OrderApi.Dtos;
using System.Text.Json;

namespace Mango.OrderApi.Services;

public class OrderPaymentStatusUpdatedServiceBusReceiver : AzureMessageBusReceiver
{
    private readonly IServiceProvider _serviceProvider;

    public OrderPaymentStatusUpdatedServiceBusReceiver(
        IServiceProvider serviceProvider,
        string topicName,
        string subscriptionName
        )
        : base(
            serviceProvider.GetRequiredService<ILogger<CheckoutMessageBusReceiver>>(),
            serviceProvider.GetRequiredService<ServiceBusClient>(),
            topicName, subscriptionName)
    {
        _serviceProvider = serviceProvider;
    }

    // handle received messages
    protected override async Task MessageHandler(ProcessMessageEventArgs args)
    {
        using var scope = _serviceProvider.CreateScope();
        var repository = scope.ServiceProvider.GetRequiredService<IOrderService>();

        var paymentResponse = JsonSerializer.Deserialize<PaymentResponseDto>(args.Message.Body.ToString());
        await repository.UpdateOrderPaymentStatus(paymentResponse!.OrderId, paymentResponse.Paid);

        // complete the message. messages is deleted from the subscription.
        await args.CompleteMessageAsync(args.Message);
    }
}