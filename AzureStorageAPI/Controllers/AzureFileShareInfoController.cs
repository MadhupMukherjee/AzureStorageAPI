using AzureStorageAPI.Service;
using Microsoft.AspNetCore.Mvc;

namespace AzureStorageAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AzureFileShareInfoController : ControllerBase
    {
        private readonly IAzureFileShare _azureFileShare;

        public AzureFileShareInfoController(IAzureFileShare azureFileShare)
        {
            _azureFileShare = azureFileShare ?? throw new ArgumentNullException(nameof(_azureFileShare));
        }



        [HttpPost]
        [Route("FileShare")]

        public async Task<IActionResult> FileShareAsync(string directoryName, string shareName, IFormFile files)
        {
            if (files != null)
            {
                return Ok(await _azureFileShare.FileShareAsync(directoryName, files.FileName, shareName, files.OpenReadStream()));
            }
            else
                return BadRequest("Not Valid input");

        }

        [HttpGet]
        [Route("DownloadFile")]
        
        public async Task<IActionResult> DownloadFileAsync(string directoryName, string shareName,string fileName)
        {
            return Ok(await _azureFileShare.DownloadFileAsync(directoryName, fileName, shareName));

        }

        [HttpDelete]
        [Route("DeleteFile")]
        public async Task<IActionResult> DeleteFileAsync(string directoryName, string shareName, string fileName)
        {
            return Ok(await _azureFileShare.DeleteFileAsync(directoryName, fileName, shareName));

        }

        [HttpGet]
        [Route("GetAllShares")]
        public async Task<IActionResult> GetAllSharesAsync()
        {
            return Ok(await _azureFileShare.GetAllSharesAsync());
        }
    }
}
