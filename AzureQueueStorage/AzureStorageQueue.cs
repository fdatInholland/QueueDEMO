using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using Microsoft.Extensions.Configuration;
using QueueDEMO.QueueInterfaces;

namespace QueueDEMO.AzureQueueStorage
{
    public class AzureStorageQueue : IAzureStorageQueue
    {
        private readonly QueueClient _queueClient;
        private QueueMessage? _lastPeekedMessage;

        public AzureStorageQueue(IConfiguration configuration)
        {
            string connectionString = configuration.GetValue<string>("AzureWebJobsStorage");
            string queueName = configuration.GetValue<string>("DemoQueue");

            _queueClient = new QueueClient(connectionString, queueName);

            // Ensure queue exists - try-catch could be used here?
            _queueClient.CreateIfNotExists();
        }

        public async Task CreateMessage(string message)
        {
            await _queueClient.SendMessageAsync(message);
        }

        public async Task DeleteMessage()
        {
            if (_lastPeekedMessage == null)
                return;

            var messages = await _queueClient.ReceiveMessagesAsync(1);
            if (messages.Value.Length > 0)
            {
                var msg = messages.Value[0];
                await _queueClient.DeleteMessageAsync(msg.MessageId, msg.PopReceipt);
            }

            _lastPeekedMessage = null;
        }

        public async Task<string> PeekMessage()
        {
            var peekedMessage = await _queueClient.PeekMessagesAsync(1);

            if (peekedMessage.Value.Length > 0)
            {
                var msg = peekedMessage.Value[0];
                return msg.MessageText;
            }
            return null;
        }
    }
}
