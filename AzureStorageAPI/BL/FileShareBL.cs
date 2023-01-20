using Azure;
using Azure.Storage.Files.Shares;
using Azure.Storage.Files.Shares.Models;
using AzureStorageAPI.Service;

namespace AzureStorageAPI.BL
{
    public class FileShareBL : IAzureFileShare
    {
        private readonly IConfiguration _configuration;

        public FileShareBL(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<string> FileShareAsync(string directoryName, string filename, string shareName, Stream fileContent)
        {

            // Get a reference to a share and then create it
            ShareClient share = new ShareClient(_configuration["StorageConnectionString"], shareName);
            await share.CreateIfNotExistsAsync();

            if (await share.ExistsAsync())
            {
                // Get a reference to a directory and create it
                ShareDirectoryClient directory = share.GetDirectoryClient(directoryName);
                await directory.CreateIfNotExistsAsync();

                if (await directory.ExistsAsync())
                {
                    // Get a reference to a file and upload it
                    ShareFileClient file = directory.GetFileClient(filename);
                    //if (await file.ExistsAsync())
                    //{
                    file.Create(fileContent.Length);
                    await file.UploadRangeAsync(
                        new HttpRange(0, fileContent.Length),
                        fileContent);
                    //}
                }
                else
                {
                    throw new InvalidOperationException("No directory Found");
                }
            }
            else
            {
                throw new InvalidOperationException("No share Client Found");
            }

            return "File Shared SuccessFully";


        }
        
        public async Task<string> DownloadFileAsync(string directoryName, string filename, string shareName)
        {
            string msg = string.Empty;
            ShareClient share = new ShareClient(_configuration["StorageConnectionString"], shareName);
            if (await share.ExistsAsync())
            {
                ShareDirectoryClient directory = share.GetDirectoryClient(directoryName);
                if (await directory.ExistsAsync())
                {
                    ShareFileClient file = directory.GetFileClient(filename);
                    if (await file.ExistsAsync())
                    {

                        // Download the file
                        ShareFileDownloadInfo download = file.Download();
                        msg = "File Downloaded SuccessFully";
                        //using (FileStream stream = File.OpenWrite(localFilePath))
                        //{
                        //    await download.Content.CopyToAsync(stream);


                        //}
                    }
                    else
                    {
                        throw new InvalidOperationException("No File Found");
                    }
                }
                else
                {
                    throw new InvalidOperationException("No directory Found");
                }

            }
            else
            {
                throw new InvalidOperationException("No Share Client Found");
            }
            return msg;
        }


        public async Task<string> DeleteFileAsync(string directoryName, string filename, string shareName)
        {
            string msg = string.Empty;
            ShareClient share = new ShareClient(_configuration["StorageConnectionString"], shareName);
            if (await share.ExistsAsync())
            {
                ShareDirectoryClient directory = share.GetDirectoryClient(directoryName);
                if (await directory.ExistsAsync())
                {
                    ShareFileClient file = directory.GetFileClient(filename);
                    if (await file.ExistsAsync())
                    {

                        await file.DeleteAsync();
                        return "File Deleted successfully";

                    }
                    else
                    {
                        throw new InvalidOperationException("No File Found");
                    }
                }
                else
                {
                    throw new InvalidOperationException("No directory Found");
                }

            }
            else
            {
                throw new InvalidOperationException("No Share Client Found");
            }
        }

        public async Task<List<string>> GetAllSharesAsync()
        {
            
            ShareServiceClient shareServiceClient = new ShareServiceClient(_configuration["StorageConnectionString"]);


            List<string> files = new List<string>();

            await foreach (ShareItem shareItem in shareServiceClient.GetSharesAsync(ShareTraits.All,ShareStates.Snapshots))
            {
               
                files.Add(shareItem.Name);
            }

            if (files.Count > 0)
            {
                return files;
            }
            else
            {
                throw new InvalidOperationException("No File Found");
            }
        }


    }
}
