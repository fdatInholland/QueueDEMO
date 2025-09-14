using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
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

            // Ensure queue exists 
            try
            {
                _queueClient.CreateIfNotExists();
            }
            catch (StorageException ex)
            {
                //Logger.LogError($"Error creating queue: {ex.Message}");
            }
            
        }

        public async Task CreateMessage(string message)
        {
            await _queueClient.SendMessageAsync(message);
        }

        public async Task DeleteMessage()
        {
           //
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
