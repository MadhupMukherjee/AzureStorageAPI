using AzureStorageAPI.Model;
using AzureStorageAPI.Service;
using Microsoft.AspNetCore.Mvc;

namespace AzureStorageAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AzureQueueInfoController : ControllerBase
    {
        private readonly IAzureQueueService _azureQueueService;
        public AzureQueueInfoController(IAzureQueueService azureQueueService)
        {
            _azureQueueService = azureQueueService ?? throw new ArgumentNullException(nameof(azureQueueService));
        }

        [HttpGet]
        [Route("PeekMessage")]
        public async Task<IActionResult> PeekMessageAsync([FromQuery] string queueName)
        {
            return Ok(await _azureQueueService.PeekMessageAsync(queueName));
        }

        [HttpPost]
        [Route("InsertMessage")]
        public async Task<IActionResult> InsertMessageAsync([FromBody] QueueModel queue)
        {
            return Ok(await _azureQueueService.InsertMessageAsync(queue));
        }
        [HttpPut]
        [Route("UpdateMessage")]
        public async Task<IActionResult> UpdateMessageAsync([FromBody] QueueModel queue)
        {
            return Ok(await _azureQueueService.UpdateMessageAsync(queue));
        }
        [HttpDelete]
        [Route("DeleteMessage")]
        public async Task<IActionResult> DeleteMessageAsync([FromQuery] string queueName)
        {
            return Ok(await _azureQueueService.DeleteMessageAsync(queueName));
        }
    }
}

