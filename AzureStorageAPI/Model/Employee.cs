using Azure;
using Azure.Data.Tables;

namespace AzureStorageAPI.Model
{


    public class Employee : ITableEntity
    {
        public Employee()
        {
            Id = String.Empty;
            Name = String.Empty;
            Department = String.Empty;
            Salary = 0;
            PartitionKey = String.Empty;
            RowKey = String.Empty;
            EmployeeCode = String.Empty;
        }
        public string Id { get; set; }
        public string EmployeeCode { get; set; }
        public string Name { get; set; }
        public string Department { get; set; }
        public int Salary { get; set; }
    
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }
    }

}
