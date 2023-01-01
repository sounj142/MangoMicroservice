using Azure.Messaging.ServiceBus;
using Mango.MessageBus;
using Mango.PaymentApi.Dtos;

namespace Mango.PaymentApi.Services;

public class OrderPaymentStatusUpdatedServiceBusSender : AzureMessageBusSender
{
    public OrderPaymentStatusUpdatedServiceBusSender(ServiceBusClient client, string topicName)
        : base(client, topicName)
    {
    }

    public Task PublishMessage(PaymentResponseDto message)
    {
        return PublishMessage<PaymentResponseDto>(message);
    }
}