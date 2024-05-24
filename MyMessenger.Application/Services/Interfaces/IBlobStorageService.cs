
namespace MyMessenger.Application.Services.Interfaces
{
    public interface IBlobStorageService
    {
        public Task<string> UploadImageAsync(Stream imageStream, string containerName, string fileName);
    }
}
