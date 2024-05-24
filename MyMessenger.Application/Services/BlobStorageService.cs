using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using MyMessenger.Application.Services.Interfaces;

namespace MyMessenger.Application.Services
{
    public class BlobStorageService : IBlobStorageService
    {
        private readonly BlobServiceClient blob;
        public BlobStorageService(BlobServiceClient blob)
        {
            this.blob = blob;
        }
        public async Task<string> UploadImageAsync(Stream imageStream, string containerName, string fileName)
        {
            var blobContainerClient = blob.GetBlobContainerClient("avatar");

            await blobContainerClient.CreateIfNotExistsAsync();

            await foreach (BlobItem blobItem in blobContainerClient.GetBlobsAsync())
            {
                if (blobItem.Name.StartsWith($"{containerName}/"))
                {
                    var blobClient = blobContainerClient.GetBlobClient(blobItem.Name);
                    await blobClient.DeleteIfExistsAsync();
                }
            }
            var newBlobClient = blobContainerClient.GetBlobClient($"{containerName}/user_avatar{Path.GetExtension(fileName)}");
            await newBlobClient.UploadAsync(imageStream, true);

            return newBlobClient.Uri.ToString();
        }
    }
}
