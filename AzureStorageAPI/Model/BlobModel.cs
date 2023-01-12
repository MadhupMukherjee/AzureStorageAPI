namespace AzureStorageAPI.Model
{
    public class BlobModel
    {
        public BlobModel()
        {
            containerName = string.Empty;
        }
        public string containerName { get; set; }
        public IFormFile files { get; set; }
    }
}
