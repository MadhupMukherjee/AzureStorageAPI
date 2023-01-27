using AzureStorageAPI.Model;
using AzureStorageAPI.Service;
using Microsoft.AspNetCore.Mvc;

namespace AzureStorageAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EmployeeInfoController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;
        public EmployeeInfoController(IEmployeeService employeeService)
        {
            _employeeService = employeeService ?? throw new ArgumentNullException(nameof(employeeService));
        }

        [HttpGet]
        [Route("GetEmployee")]
        public async Task<IActionResult> GetEmployee([FromQuery] string department, string id)
        {
            return Ok(await _employeeService.GetEmployeeAsync(department, id));

        }
        [HttpPost]
        [Route("AddEmployeeAsync")]
        public async Task<IActionResult> AddEmployeeAsync([FromBody] Employee employee)
        {
            employee.PartitionKey = employee.Department;
            string Id = Guid.NewGuid().ToString();
            employee.Id = Id;
            employee.RowKey = Id;
            var createdEntity = await _employeeService.UpsertEmployeeAsync(employee);
            return CreatedAtAction(nameof(GetEmployee), createdEntity);
        }
        [HttpPut]
        [Route("EditEmployeeAsync")]
        public async Task<IActionResult> EditEmployeeAsync([FromBody] Employee employee)
        {
            employee.PartitionKey = employee.Department;
            employee.RowKey = employee.Id;
            return Ok(await _employeeService.UpsertEmployeeAsync(employee));
        }
        [HttpDelete]
        [Route("DeleteEmployee")]
        public async Task<IActionResult> DeleteEmployee([FromQuery] string department, string id)
        {
            return Ok(await _employeeService.DeleteEmployeeAsync(department, id));
            
        }
    }
}
