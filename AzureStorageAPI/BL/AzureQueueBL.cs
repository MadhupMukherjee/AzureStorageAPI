using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using AzureStorageAPI.Model;
using AzureStorageAPI.Service;

namespace AzureStorageAPI.BL
{
    public class AzureQueueBL : IAzureQueueService
    {
        private readonly IConfiguration _configuration;

        public AzureQueueBL(IConfiguration configuration)
        {
            _configuration = configuration;    
        }

        private async Task<QueueClient> GetQueueClient(string queuename)
        {
            // Instantiate a QueueClient which will be used to create and manipulate the queue
            QueueClient queueClient = new QueueClient(_configuration["StorageConnectionString"], queuename);
            // Create the queue
            await queueClient.CreateIfNotExistsAsync();
            return queueClient;

        }

        public async Task<string> InsertMessageAsync(QueueModel queue)
        {
            var queueClient = await GetQueueClient(queue.queueName);
            string returnmessage = string.Empty;
            if (await  queueClient.ExistsAsync())
            {
                // Send a message to the queue
                await queueClient.SendMessageAsync(queue.message);

                returnmessage = "message Send successfully";

            }
            else
            {
                throw new InvalidOperationException("queueName Not Found");
            }
            return returnmessage;
        }
        public async Task<string> UpdateMessageAsync(QueueModel queue)
        {
            var queueClient = await GetQueueClient(queue.queueName);
            string returnmessage = string.Empty;
            if (await queueClient.ExistsAsync())
            {
                // Get the message from the queue
                QueueMessage[] queueMessage = queueClient.ReceiveMessages();

                // Update the message contents
                queueClient.UpdateMessage(queueMessage[0].MessageId,
                        queueMessage[0].PopReceipt,
                        queue.message,
                        TimeSpan.FromSeconds(60.0)  // Make it invisible for another 60 seconds
                    );

                returnmessage = "message Updated successfully";

            }
            else
            {
                throw new InvalidOperationException("queueName Not Found");
            }
            return returnmessage;
        }
        public async Task<string> PeekMessageAsync(string queueName)
        {
            var queueClient = await GetQueueClient(queueName);
            string returnmessage = string.Empty;
            if (await queueClient.ExistsAsync())
            {
                // Peek at the next message
                PeekedMessage[] peekedMessage = queueClient.PeekMessages();

                if (peekedMessage==null)
                    throw new InvalidOperationException("No Message Found");
                else
                    returnmessage = peekedMessage[0].Body.ToString();

            }
            else
            {
                throw new InvalidOperationException("queueName Not Found");
            }
            return returnmessage;
        }

        public async Task<string> DeleteMessageAsync(string queueName)
        {
            var queueClient = await GetQueueClient(queueName);
            string returnmessage = string.Empty;
            if (await  queueClient.ExistsAsync())
            {
                // Get the next message
                QueueMessage[] retrievedMessage = queueClient.ReceiveMessages();
                if (retrievedMessage == null)
                    throw new InvalidOperationException("No Message Found");
                else
                    await queueClient.DeleteMessageAsync(retrievedMessage[0].MessageId, retrievedMessage[0].PopReceipt);

                returnmessage = "message deleted successfully";

            }
            else
            {
                throw new InvalidOperationException("queueName Not Found");
            }
            return returnmessage;
        }



    }
}
