using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Forms;
using MyMessenger.Maui.Models;
using MyMessenger.Maui.Library.Interface;
using System.Net.Http.Json;

namespace MyMessenger.Maui.Services
{
    public class UserService : GenericService<UserDTO>
    {
        public UserService(IHttpWrapper httpWrapper, ILocalStorageService storage) : base(httpWrapper, storage)
        {
            
        }
        public async Task<UserDTO> GetUserInfo()
        {
            var accessToken = await storage.GetItemAsStringAsync("accessToken");
            var response = await httpWrapper.GetAsync("User/current", accessToken);
            var result = await response.Content.ReadFromJsonAsync<UserDTO>();
            return result;
        }
        public async Task<string?> UploadImage(IBrowserFile selectedFile)
        {
            try
            {
                if (selectedFile != null)
                {
                    using (var stream = selectedFile.OpenReadStream(maxAllowedSize: 1366 * 768))
                    {
                        var content = new MultipartFormDataContent();
                        content.Add(new StreamContent(stream), "image", selectedFile.Name);
                        var accessToken = await storage.GetItemAsStringAsync("accessToken");
                        var response = await httpWrapper.PostImageAsync("User/", content, accessToken);
                        var result = await response.Content.ReadAsStringAsync();
                        return result;
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task UpdateInfo(string name, string phonenumber)
        {
            var accessToken = await storage.GetItemAsStringAsync("accessToken");
            await httpWrapper.PutAsync($"User/{name}/{phonenumber}", "", accessToken);
        }
    }
}
