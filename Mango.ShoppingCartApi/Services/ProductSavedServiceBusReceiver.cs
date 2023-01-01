using Azure.Messaging.ServiceBus;
using Mango.MessageBus;
using Mango.ShoppingCartApi.Dtos;
using System.Text.Json;

namespace Mango.ShoppingCartApi.Services;

public class ProductSavedServiceBusReceiver : AzureMessageBusReceiver
{
    private readonly IServiceProvider _serviceProvider;

    public ProductSavedServiceBusReceiver(
        IServiceProvider serviceProvider,
        string topicName,
        string subscriptionName
        )
        : base(
            serviceProvider.GetRequiredService<ILogger<ProductSavedServiceBusReceiver>>(),
            serviceProvider.GetRequiredService<ServiceBusClient>(),
            topicName, subscriptionName)
    {
        _serviceProvider = serviceProvider;
    }

    // handle received messages
    protected override async Task MessageHandler(ProcessMessageEventArgs args)
    {
        using var scope = _serviceProvider.CreateScope();
        var productService = scope.ServiceProvider.GetRequiredService<IProductService>();

        var product = JsonSerializer.Deserialize<ProductDto>(args.Message.Body.ToString());
        await productService.CreateOrUpdateProduct(product!);

        // complete the message. messages is deleted from the subscription.
        await args.CompleteMessageAsync(args.Message);
    }
}