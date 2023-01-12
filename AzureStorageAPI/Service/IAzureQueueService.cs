using AzureStorageAPI.Model;

namespace AzureStorageAPI.Service
{
    public interface IAzureQueueService
    {
        Task<string> InsertMessageAsync(QueueModel queue);
        Task<string> PeekMessageAsync(string queueName);
        Task <string> UpdateMessageAsync(QueueModel queue);
        Task<string> DeleteMessageAsync(string queueName);
    }
}
