using AzureStorageAPI.Model;
using AzureStorageAPI.Service;
using Microsoft.AspNetCore.Mvc;

namespace AzureStorageAPI.Controllers
{
    [ApiController]
    [Route("[controller]")] 
    public class AzureBlobInfoController : ControllerBase
    {


        private readonly IAzureBlobService _azureBlobService;

        public AzureBlobInfoController(IAzureBlobService azureBlobService)
        {
            _azureBlobService = azureBlobService ?? throw new ArgumentNullException(nameof(_azureBlobService));
        }

        [HttpPost]
        [Route("UploadBlob")]
        public async Task<IActionResult> UploadBlobAsync(string containerName, IFormFile files)
        {
            if (files != null)
            {
                return Ok(await _azureBlobService.UploadBlobAsync(containerName, files.FileName, files.OpenReadStream()));
            }
            else
                return BadRequest("Not Valid input");



        }

        [HttpGet]
        [Route("GetAllDocuments")]
        public async Task<IActionResult> GetAllDocumentsAsync([FromQuery] string containerName)
        {
            return Ok(await _azureBlobService.GetAllDocumentsAsync(containerName));
        }
        [HttpGet]
        [Route("GetDocument")]
        public async Task<IActionResult> GetDocumentAsync([FromQuery] string containerName, string filename)
        {
            return Ok(await _azureBlobService.GetDocumentAsync(containerName, filename));
        }
        [HttpDelete]
        [Route("DeleteDocument")]
        public async Task<IActionResult> DeleteDocumentAsync([FromQuery] string containerName, string filename)
        {
            return Ok(await _azureBlobService.DeleteDocumentAsync(containerName, filename));
        }
    }
}
