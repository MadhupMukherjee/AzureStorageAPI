﻿namespace AzureStorageAPI.Service
{
    public interface IAzureFileShare
    {
        Task<string> FileShareAsync(string directoryName, string filename, string shareName, Stream fileContent);

        Task<string> DownloadFileAsync(string directoryName, string filename, string shareName);
    }
}