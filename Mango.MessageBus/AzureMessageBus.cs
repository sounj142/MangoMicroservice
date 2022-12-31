using Azure.Messaging.ServiceBus;
using System.Text;
using System.Text.Json;

namespace Mango.MessageBus;

public class AzureMessageBus
{
    private readonly ServiceBusClient _client;
    private readonly string _topicName;

    public AzureMessageBus(ServiceBusClient client, string topicName)
    {
        _client = client;
        _topicName = topicName;
    }

    public async Task PublishMessage<T>(T message) where T : class
    {
        await using var sender = _client.CreateSender(_topicName);
        var textMessage = JsonSerializer.Serialize(message);

        await sender.SendMessageAsync(
            new ServiceBusMessage(Encoding.UTF8.GetBytes(textMessage)));
    }
}