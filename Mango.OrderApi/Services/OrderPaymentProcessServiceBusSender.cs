using Azure.Messaging.ServiceBus;
using Mango.MessageBus;
using Mango.OrderApi.Dtos;

namespace Mango.OrderApi.Services;

public class OrderPaymentProcessServiceBusSender : AzureMessageBusSender
{
    public OrderPaymentProcessServiceBusSender(ServiceBusClient client, string topicName)
        : base(client, topicName)
    {
    }

    public Task PublishMessage(PaymentRequestDto message)
    {
        return PublishMessage<PaymentRequestDto>(message);
    }
}