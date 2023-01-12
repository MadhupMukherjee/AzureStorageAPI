using AzureStorageAPI.Model;

namespace AzureStorageAPI.Service
{
    public interface IAzureBlobService
    {
        Task<string> UploadBlobAsync(string containerName, string filename,Stream fileContent);
        Task<Stream> GetDocumentAsync(string containerName, string filename);
        Task<List<string>> GetAllDocumentsAsync(string containerName);
        Task<string> DeleteDocumentAsync(string containerName, string filename);
    }
}
