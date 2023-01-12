namespace AzureStorageAPI.BL
{
    public class CustomError
    {
        private string message { get; set; }
        public enum customErrorType { get; set; }
        public CustomError(int _customErrorType,string _error)
        {
            message = _error;
            customErrorType = _customErrorType;
        }
    }
    public enum CustomErrorType
    {
        NoDataFound=,
        Exception,
        FileNotFound,
        Validation
    }
}
