using Azure.Messaging.ServiceBus;
using Mango.MessageBus;

namespace Mango.ShoppingCartApi.Services;

public class CheckoutMessageBusSender : AzureMessageBusSender
{
    public CheckoutMessageBusSender(ServiceBusClient client, string topicName)
        : base(client, topicName)
    {
    }
}