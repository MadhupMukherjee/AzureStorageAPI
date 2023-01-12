namespace AzureStorageAPI.Model
{
    public class QueueModel
    {
        public QueueModel()
        {
            queueName = string.Empty;
            message = string.Empty;
        }
        public string queueName { get; set; }
        public string message { get; set; }
    }
}
