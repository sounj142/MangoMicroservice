using Azure.Messaging.ServiceBus;
using System.Text;
using System.Text.Json;

namespace Mango.MessageBus;

public class AzureMessageBusSender : IAsyncDisposable
{
    private readonly ServiceBusSender _sender;

    public AzureMessageBusSender(ServiceBusClient client, string topicName)
    {
        _sender = client.CreateSender(topicName);
    }

    public async Task PublishMessage<T>(T message) where T : class
    {
        var textMessage = JsonSerializer.Serialize(message);

        await _sender.SendMessageAsync(
            new ServiceBusMessage(Encoding.UTF8.GetBytes(textMessage)));
    }

    public async ValueTask DisposeAsync()
    {
        await _sender.DisposeAsync();
    }
}