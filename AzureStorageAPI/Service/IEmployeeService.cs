using AzureStorageAPI.Model;

namespace AzureStorageAPI.Service
{
    public interface IEmployeeService
    {
        Task<Employee> GetEmployeeAsync(string department,string id);
        Task<Employee> UpsertEmployeeAsync(Employee employee);
        Task<string> DeleteEmployeeAsync(string department, string id);
    }
}
