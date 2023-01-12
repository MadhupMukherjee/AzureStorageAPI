namespace AzureStorageAPI.Model
{
    public class ErrorDetails
    {
        public ErrorDetails()
        {
            StatusCode = 0;
            message = string.Empty;
        }
        public int StatusCode { get; set; }
        public string message { get; set; }

    }
}
