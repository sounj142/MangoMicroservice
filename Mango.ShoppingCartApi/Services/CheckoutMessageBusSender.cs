using Azure.Messaging.ServiceBus;
using Mango.MessageBus;
using Mango.ShoppingCartApi.Dtos;

namespace Mango.ShoppingCartApi.Services;

public class CheckoutMessageBusSender : AzureMessageBusSender
{
    public CheckoutMessageBusSender(ServiceBusClient client, string topicName)
        : base(client, topicName)
    {
    }

    public Task PublishMessage(CheckoutDto message)
    {
        return PublishMessage<CheckoutDto>(message);
    }
}