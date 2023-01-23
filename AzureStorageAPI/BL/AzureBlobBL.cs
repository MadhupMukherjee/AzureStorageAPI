using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using AzureStorageAPI.Model;
using AzureStorageAPI.Service;






namespace AzureStorageAPI.BL
{
    public class AzureBlobBL : IAzureBlobService
    {
        private readonly IConfiguration _configuration;

        public AzureBlobBL(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<string> DeleteDocumentAsync(string containerName, string filename)
        {
            var container = BlobExtensions.GetContainer(_configuration["StorageConnectionString"], containerName);
            if (!await container.ExistsAsync())
            {
                throw new FileNotFoundException("Container Not Found");
            }


            var blobClient = container.GetBlobClient(filename);

            if (await blobClient.ExistsAsync())
            {
                await blobClient.DeleteIfExistsAsync();
                return "File Deleted SuccessFully";
            }
            else
            {
                throw new FileNotFoundException("File Not Found");
            }
        }

        public async Task<List<string>> GetAllDocumentsAsync(string containerName)
        {
            var container = BlobExtensions.GetContainer(_configuration["StorageConnectionString"], containerName);

            if (!await container.ExistsAsync())
            {
                return new List<string>();
            }

            List<string> blobs = new List<string>();

            await foreach (BlobItem blobItem in container.GetBlobsAsync())
            {
                blobs.Add(blobItem.Name);
            }

            if (blobs.Count > 0)
            {
                return blobs;
            }
            else
            {
                throw new InvalidOperationException("No File Found");
            }
        }

        public async Task<Stream> GetDocumentAsync(string containerName, string filename)
        {
            var container = BlobExtensions.GetContainer(_configuration["StorageConnectionString"], containerName);
            if (await container.ExistsAsync())
            {
                var blobClient = container.GetBlobClient(filename);
                if (blobClient.Exists())
                {
                    var content = await blobClient.DownloadStreamingAsync();
                    return content.Value.Content;
                }
                else
                {
                    throw new InvalidOperationException("No File Found");
                }
            }
            else
            {
                throw new InvalidOperationException("No container Found");
            }
        }

        public async Task<string> UploadBlobAsync(string containerName, string filename, Stream fileContent)
        {
            var container = BlobExtensions.GetContainer(_configuration["StorageConnectionString"], containerName);
            if (!await container.ExistsAsync())
            {
                await GetBlobClient(containerName);
                container = BlobExtensions.GetContainer(_configuration["StorageConnectionString"], containerName);
            }

            var bobclient = container.GetBlobClient(filename);
            if (!bobclient.Exists())
            {
                fileContent.Position = 0;
                await container.UploadBlobAsync(filename, fileContent);
                return "File Uploaded SuccessFully";
            }
            else
            {
                fileContent.Position = 0;
                await bobclient.UploadAsync(fileContent, overwrite: true);
                return "File Uploaded SuccessFully";
            }
        }

        private async Task<BlobServiceClient> GetBlobClient(string containerName)
        {
      
            BlobServiceClient blobServiceClient = new BlobServiceClient(_configuration["StorageConnectionString"]);
            
            var container = BlobExtensions.GetContainer(_configuration["StorageConnectionString"], containerName);
            if (!await container.ExistsAsync())
            {
                await blobServiceClient.CreateBlobContainerAsync(containerName);
            }
            return blobServiceClient;
        }

        public static class BlobExtensions
        {
            public static BlobContainerClient GetContainer(string connectionString, string containerName)
            {
                BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);
                return blobServiceClient.GetBlobContainerClient(containerName);
            }
        }
    }
}
