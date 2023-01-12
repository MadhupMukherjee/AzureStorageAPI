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
                    if (await file.ExistsAsync())
                    {
                        file.Create(fileContent.Length);
                        await file.UploadRangeAsync(
                            new HttpRange(0, fileContent.Length),
                            fileContent);
                    }
                }
            }

            return "File Shared SuccessFully";



            //// Instantiate a ShareClient which will be used to create and manipulate the file share
            //ShareClient share = new ShareClient(_configuration["StorageConnectionString"], shareName);

            //// Create the share if it doesn't already exist
            //await share.CreateIfNotExistsAsync();

            //// Ensure that the share exists
            //if (await share.ExistsAsync())
            //{ 

            //    // Get a reference to the sample directory
            //    ShareDirectoryClient directory = share.GetDirectoryClient(directoryName);

            //    // Create the directory if it doesn't already exist
            //    await directory.CreateIfNotExistsAsync();

            //    // Ensure that the directory exists
            //    if (await directory.ExistsAsync())
            //    {
            //        // Get a reference to a file object
            //        ShareFileClient file = directory.GetFileClient(filename);

            //        // Ensure that the file exists
            //        if (await file.ExistsAsync())
            //        {

            //            // Download the file
            //            ShareFileDownloadInfo download = await file.DownloadAsync();

            //            // Save the data to a local file, overwrite if the file already exists
            //            using (FileStream stream = File.OpenWrite(@"downloadedLog1.txt"))
            //            {
            //                await download.Content.CopyToAsync(stream);
            //                await stream.FlushAsync();
            //                stream.Close();

            //                // Display where the file was saved
            //                Console.WriteLine($"File downloaded: {stream.Name}");
            //            }
            //        }
            //    }
            //}
            //else
            //{
            //    Console.WriteLine($"CreateShareAsync failed");
            //}
        }

        public async Task<string> DownloadFileAsync(string directoryName, string filename, string shareName)
        {
            string msg = string.Empty;
            string localFilePath = @"E:\Project\AzureStorageAPI\AzureStorageAPI\Files"; ;
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
                }

            }

            return msg;
        }

        //public async Task<string> GetFileListsAsync(string directoryName)
        //{
        //    string msg = string.Empty;
        //    string localFilePath = @"E:\Project\AzureStorageAPI\AzureStorageAPI\Files"; ;
        //    ShareFileItem shareFileItem = null;
        //    ShareClient share = new ShareClient(_configuration["StorageConnectionString"], shareName);
        //    if (await share.ExistsAsync())
        //    {
        //        ShareDirectoryClient directory = share.GetDirectoryClient(directoryName);
        //        if (await directory.ExistsAsync())
        //        {
        //            shareFileItem = directory.GetFilesAndDirectories(directoryName);
        //        }

        //    }

        //    return msg;
        //}
    }
}
