using Microsoft.AspNetCore.Components.Forms;
using MyMessenger.Application.DTO.UserDTOs;

namespace MyMessenger.Maui.Services.PageService
{
    public class UserSettingsService
    {
        public UserDTO user { get; set; }
        private UserService userService;
        private IBrowserFile selectedFile;

        public string selectedFileBase64;
        public string imageUrl;
        public int avatarWidth = 100;
        public int avatarHeight = 100;

        public event Action OnDataChanged;
        public UserSettingsService(UserService userService)
        {
            this.userService = userService;
            user = new UserDTO();
            GetInfo();
        }
        public async void GetInfo()
        {
            user = await userService.GetUserInfo();
            OnDataChanged?.Invoke();
        }
        public async Task DownloadImage(InputFileChangeEventArgs e)
        {
            selectedFile = e.File;
            await userService.UploadImage(selectedFile);
            GetInfo();
        }
        public async Task UpdateInfo()
        {
            await userService.UpdateInfo(user.Name, user.PhoneNumber);
            GetInfo();
        }
    }
}
