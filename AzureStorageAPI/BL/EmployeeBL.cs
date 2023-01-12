using Azure.Data.Tables;
using AzureStorageAPI.Model;
using AzureStorageAPI.Service;

namespace AzureStorageAPI.BL
{
    public class EmployeeBL : IEmployeeService
    {
        private const string tableName = "EmployeeInformation";
        private readonly IConfiguration _configuration;
        public EmployeeBL(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private async Task<TableClient> GetTableClient()
        {
            var serviceClient = new TableServiceClient(_configuration["StorageConnectionString"]);
            var tableClient = serviceClient.GetTableClient(tableName);
            await tableClient.CreateIfNotExistsAsync();
            return tableClient;
        }

        public async Task<string> DeleteEmployeeAsync(string department, string id)
        {
            var tableClient = await GetTableClient();
            await tableClient.DeleteEntityAsync(department, id);
            return "Employee " + id + " deleted successfully";
        }

        public async Task<Employee> GetEmployeeAsync(string department, string id)
        {
            var tableClient = await GetTableClient();
            return await tableClient.GetEntityAsync<Employee>(department, id);
        }

        public async Task<Employee> UpsertEmployeeAsync(Employee employee)
        {
            var tableClient = await GetTableClient();
            await tableClient.UpsertEntityAsync(employee);
            return employee;
        }

        

       
    }
}
