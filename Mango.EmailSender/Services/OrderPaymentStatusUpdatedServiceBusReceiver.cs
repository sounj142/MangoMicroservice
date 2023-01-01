using Azure.Messaging.ServiceBus;
using Mango.EmailSender.Dtos;
using Mango.MessageBus;
using System.Text.Json;

namespace Mango.EmailSender.Services;

public class OrderPaymentStatusUpdatedServiceBusReceiver : AzureMessageBusReceiver
{
    private readonly IServiceProvider _serviceProvider;

    public OrderPaymentStatusUpdatedServiceBusReceiver(
        IServiceProvider serviceProvider,
        string topicName,
        string subscriptionName
        )
        : base(
            serviceProvider.GetRequiredService<ILogger<OrderPaymentStatusUpdatedServiceBusReceiver>>(),
            serviceProvider.GetRequiredService<ServiceBusClient>(),
            topicName, subscriptionName)
    {
        _serviceProvider = serviceProvider;
    }

    // handle received messages
    protected override async Task MessageHandler(ProcessMessageEventArgs args)
    {
        using var scope = _serviceProvider.CreateScope();
        var emailService = scope.ServiceProvider.GetRequiredService<IEmailService>();

        var paymentResponse = JsonSerializer.Deserialize<PaymentResponseDto>(args.Message.Body.ToString());
        await emailService.SendEmail(paymentResponse!);

        // complete the message. messages is deleted from the subscription.
        await args.CompleteMessageAsync(args.Message);
    }
}