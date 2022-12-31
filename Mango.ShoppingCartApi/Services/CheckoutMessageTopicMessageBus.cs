using Azure.Messaging.ServiceBus;
using Mango.MessageBus;

namespace Mango.ShoppingCartApi.Services;

public class CheckoutMessageTopicMessageBus : AzureMessageBus
{
    public CheckoutMessageTopicMessageBus(ServiceBusClient client, string topicName)
        : base(client, topicName)
    {
    }
}