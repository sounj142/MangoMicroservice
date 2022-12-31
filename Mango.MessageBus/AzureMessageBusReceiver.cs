using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Logging;

namespace Mango.MessageBus;

public abstract class AzureMessageBusReceiver : IAsyncDisposable
{
    private readonly ServiceBusProcessor _processor;
    protected readonly ILogger<AzureMessageBusReceiver> _logger;

    public AzureMessageBusReceiver(
        ILogger<AzureMessageBusReceiver> logger,
        ServiceBusClient client,
        string topicName,
        string subscriptionName)
    {
        _logger = logger;
        _processor = client.CreateProcessor(topicName, subscriptionName, new ServiceBusProcessorOptions());
    }

    public async Task Subscribe()
    {
        // add handler to process messages
        _processor.ProcessMessageAsync += MessageHandler;

        // add handler to process any errors
        _processor.ProcessErrorAsync += ErrorHandler;

        // start processing
        await _processor.StartProcessingAsync();
    }

    // handle received messages
    protected abstract Task MessageHandler(ProcessMessageEventArgs args);

    // handle any errors when receiving messages
    protected virtual Task ErrorHandler(ProcessErrorEventArgs args)
    {
        _logger.LogError(args.Exception.ToString());
        return Task.CompletedTask;
    }

    public virtual async ValueTask DisposeAsync()
    {
        await _processor.DisposeAsync();
    }
}