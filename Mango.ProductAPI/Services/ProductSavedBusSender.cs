using Azure.Messaging.ServiceBus;
using Mango.MessageBus;
using Mango.ProductAPI.Dtos;

namespace Mango.ProductAPI.Services;

public class ProductSavedBusSender : AzureMessageBusSender
{
    public ProductSavedBusSender(ServiceBusClient client, string topicName)
        : base(client, topicName)
    {
    }

    public Task PublishMessage(ProductDto message)
    {
        return PublishMessage<ProductDto>(message);
    }
}