using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using AzureBlobStorageApp.Models;
using AzureBlobStorageApp.Services;
using Microsoft.Extensions.Configuration;

namespace AzureBlobStorageApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly BlobStorageService _blobStorageService;
        //private readonly IConfiguration _configuration;

        public HomeController(BlobStorageService blobStorageService)
        {
            _blobStorageService = blobStorageService;
           // _configuration = configuration;
        }

        public async Task<IActionResult> Index()
        {
            //string containerName = _configuration.GetValue<string>("BlobContainerName");
            var images = await _blobStorageService.ListImagesAsync("your-container-name");
            var publicAccessLevel = await _blobStorageService.GetContainerPublicAccessLevelAsync("your-container-name");

            ViewBag.PublicAccessLevel = publicAccessLevel;
            return View(images);
        }

        [HttpGet("download/{imageName}")]
        public async Task<IActionResult> DownloadImage(string imageName)
        {
            //string containerName = _configuration.GetValue<string>("BlobContainerName");
            var blob = _blobStorageService.GetBlob(imageName, "your-container-name");
            var memoryStream = new MemoryStream();
            await blob.DownloadToAsync(memoryStream);
            memoryStream.Position = 0;

            return File(memoryStream, "image/jpeg", imageName);
        }

    }
}
