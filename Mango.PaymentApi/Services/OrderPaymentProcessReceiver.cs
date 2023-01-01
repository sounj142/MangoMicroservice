using AutoMapper;
using Azure.Messaging.ServiceBus;
using Mango.MessageBus;
using Mango.PaymentApi.Dtos;
using PaymentProcessor;
using System.Text.Json;

namespace Mango.PaymentApi.Services;

public class OrderPaymentProcessReceiver : AzureMessageBusReceiver
{
    private readonly IServiceProvider _serviceProvider;

    public OrderPaymentProcessReceiver(
        IServiceProvider serviceProvider,
        string topicName,
        string subscriptionName
        )
        : base(
            serviceProvider.GetRequiredService<ILogger<OrderPaymentProcessReceiver>>(),
            serviceProvider.GetRequiredService<ServiceBusClient>(),
            topicName, subscriptionName)
    {
        _serviceProvider = serviceProvider;
    }

    // handle received messages
    protected override async Task MessageHandler(ProcessMessageEventArgs args)
    {
        using var scope = _serviceProvider.CreateScope();

        var paymentStatusUpdatedSender = scope.ServiceProvider.GetRequiredService<OrderPaymentStatusUpdatedServiceBusSender>();
        var processPayment = scope.ServiceProvider.GetRequiredService<IProcessPayment>();
        var mapper = scope.ServiceProvider.GetRequiredService<IMapper>();

        var paymentRequest = JsonSerializer.Deserialize<PaymentRequestDto>(
            args.Message.Body.ToString());

        var isPaid = await processPayment.PaymentProcessor(
            mapper.Map<PaymentRequest>(paymentRequest));
        var paymentResponse = new PaymentResponseDto
        {
            OrderId = paymentRequest!.OrderId,
            Paid = isPaid,
            Email = paymentRequest.Email,
            FirstName = paymentRequest.FirstName,
            LastName = paymentRequest.LastName,
        };
        await paymentStatusUpdatedSender.PublishMessage(paymentResponse);

        // complete the message. messages is deleted from the subscription.
        await args.CompleteMessageAsync(args.Message);
    }
}