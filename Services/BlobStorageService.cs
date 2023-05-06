namespace AzureBlobStorageApp.Services;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using AzureBlobStorageApp.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Threading.Tasks;

public class BlobStorageService
{
    private readonly BlobServiceClient _blobServiceClient;
    private const string ContainerName = "your-container-name";

    public BlobStorageService(string connectionString)
    {
        _blobServiceClient = new BlobServiceClient(connectionString);
    }

    public async Task<List<ImageModel>> ListImagesAsync(string containerName)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
        var images = new List<ImageModel>();

        await foreach (var blobItem in containerClient.GetBlobsAsync())
        {
            var blobClient = containerClient.GetBlobClient(blobItem.Name);
            images.Add(new ImageModel { Name = blobItem.Name, Uri = blobClient.Uri });
        }

        return images;
    }

    public BlobClient GetBlob(string blobName, string containerName)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
        var blobClient = containerClient.GetBlobClient(blobName);

        return blobClient;
    }

    public async Task<PublicAccessType> GetContainerPublicAccessLevelAsync(string containerName)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
        var response = await containerClient.GetPropertiesAsync();
        return response.Value.PublicAccess ?? PublicAccessType.None;
    }
}

